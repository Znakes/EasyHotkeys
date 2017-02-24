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
        /// Уникальная строка. Используется для сохранения конфигурации.
        /// Маска: ClassName.MethodName
        /// Если поле не будет заполнено, оно заполнится так во время поиска.
        /// </summary>
        public string UniqueString { get; set; }

        public Key DefaultKey { get; set; } = Key.None;
        public ModifierKeys DefaultKeyModifier { get; set; } = ModifierKeys.None;
    }
}
