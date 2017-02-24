using System;
using System.Windows.Input;

namespace EasyHotkeys
{
    /// <summary>
    ///     Marks function as hotkey's callable
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class HotkeyObservableAttribute : Attribute
    {
        /// <summary>
        ///     Description. Displays in UI
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Unique string. Used for storing in cofiguration map
        ///     Preffered mask: ClassName.MethodName
        ///     (If empty, will filled by mask on searching step)
        /// </summary>
        public string UniqueString { get; set; }

        /// <summary>
        ///     Pressed Key
        /// </summary>
        public Key DefaultKey { get; set; } = Key.None;

        /// <summary>
        ///     Pressed Modifiers
        /// </summary>
        public ModifierKeys DefaultKeyModifier { get; set; } = ModifierKeys.None;
    }
}