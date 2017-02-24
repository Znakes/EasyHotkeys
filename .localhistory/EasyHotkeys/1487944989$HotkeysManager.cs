using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Xml.Serialization;
using EasyHotkeys.Utils;

namespace EasyHotkeys
{
    [Serializable]
    public class InvokableMethod
    {
        public object Target { get; set; }

        [XmlIgnore]
        public MethodInfo Method { get; set; }

        public string Description { get; set; }

        public string UniqueString { get; set; }

        public Key Key { get; set; }
        public ModifierKeys Modifier { get; set; }

        public override string ToString()
        {
            return $"{Modifier}+{Key}:{Description}";
        }
    }

    public static class TypeExtensions
    {
        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }

    public class KeyWithModifiers
    {
        public Key Key { get; set; }
        public ModifierKeys Modifier { get; set; }
    }

    public class HotkeysManager
    {
        const string DefaultFilename = "hotkeys.ini";
        private readonly object[] _targets;

        public bool Enabled { get; set; } = true;

        [XmlIgnore]
        public InvokableMethod[] Methods { get; set; }

        public SerializableDictionary<KeyWithModifiers, InvokableMethod> Bindings { get; set; } = new SerializableDictionary<KeyWithModifiers, InvokableMethod>();

        [Obsolete("Don't use this. It was made for serialization purposes!")]
        public HotkeysManager()
        {

        }

        public HotkeysManager(object[] targets)
        {
            _targets = targets;
            SearchInAssembly();

            foreach (var method in Methods.Where(m => m.Key != Key.None))
            {
                Bindings.Add(new KeyWithModifiers()
                {
                    Key = method.Key,
                    Modifier = method.Modifier
                },
                method);
            }
        }

        public void Save(string filename = DefaultFilename)
        {
            try
            {
                using (var fileWriter = new FileStream(filename, FileMode.Create))
                {
                    var xmlSerializer = new XmlSerializer(typeof(HotkeysManager));
                    xmlSerializer.Serialize(fileWriter, this);
                }
            }
            catch (Exception ex)
            {

#if DEBUG
                Debug.WriteLine(ex.Message);

#endif
            }
        }

        public bool TryRaiseHotkey(Key key, ModifierKeys modifier)
        {
            var match = Bindings?.Where(b => b.Key.Key == key && modifier == b.Key.Modifier).ToArray();

            if (!match?.Any() == true)
                return false;

            Invoke(match.First().Value);

            return true;
        }

        public void Load(string filename = DefaultFilename)
        {
            try
            {
                if (!File.Exists(filename))
                    return;

                HotkeysManager newInstance = null;
                using (var fileWriter = new FileStream(filename, FileMode.Open))
                {
                    var xmlSerializer = new XmlSerializer(typeof(HotkeysManager));
                    newInstance = (HotkeysManager)xmlSerializer.Deserialize(fileWriter);
                }

                if (newInstance?.Bindings != null)
                {

                    Bindings.Clear();

                    foreach (var binding in newInstance.Bindings)
                    {
                        //serach in available methods
                        var method = Methods.FirstOrDefault(m => m.UniqueString == binding.Value.UniqueString);
                        if (method != null)
                        {
                            method.Key = binding.Value.Key;
                            method.Modifier = binding.Value.Modifier;
                            Bindings.Add(binding.Key, method);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);

#endif
            }

        }

        private void SearchInAssembly()
        {
            Methods = Methods ?? (
                from type in Assembly.GetExecutingAssembly().GetTypes()
                from method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                from attribute in
                    method.GetCustomAttributes(typeof(HotkeyObservableAttribute), false)
                        .OfType<HotkeyObservableAttribute>()
                select new InvokableMethod
                {
                    Method = method,
                    Description = attribute.Description,
                    UniqueString =
                        string.IsNullOrEmpty(attribute.UniqueString) ?
                        method.DeclaringType.FullName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last() :
                        attribute.UniqueString,
                    Key = attribute.DefaultKey,
                    Modifier = attribute.DefaultKeyModifier
                    //Target = type
                }).ToArray();
        }

        private void Invoke(InvokableMethod method)
        {
            var target = _targets.FirstOrDefault(f => f.GetType() == method.Method.DeclaringType);

            if (target == null)
            {


#if DEBUG
                Debug.WriteLine($"Can't find target with the same type ({method.Method.Name})");

#endif
                return;
            }

            Logger.LogAsync(method.Method.Name);

            var args = method.Method.GetParameters();

            var arguments = new List<object>(args?.Length ?? 0);
            if (args.Any())
            {
                arguments.AddRange(args.Select(info => info.ParameterType.GetDefault()));
            }

            method.Method.Invoke(target, arguments.ToArray());
        }
    }
}