using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyHotkeys
{
    /// <summary>
    /// Marks function as hotkey's callable
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class HotkeyObservableAttribute : Attribute
    {
        /// <summary>
        /// Description. Displays in UI
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unique string. Used for storing in cofiguration map
        /// Preffered mask: ClassName.MethodName
        /// (If empty, will filled by mask on searching step)
        /// </summary>
        public string UniqueString { get; set; }

        public Key DefaultKey { get; set; } = Key.None;
        public ModifierKeys DefaultKeyModifier { get; set; } = ModifierKeys.None;
    }
}
