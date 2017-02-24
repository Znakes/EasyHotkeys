# EasyHotkeys

Lite hotkeys manager for wpf apps.

Hot to use: 

* Mark functon as hotkey callable
```
        /// <summary>
        ///     Checks availability of next and prev sessions to the current selected
        /// </summary>
        [HotkeyObservable(Description = "Refresh list", UniqueString = "MainWindow.UpdateNextPrevSessionsBtns", DefaultKey = Key.F5)]

        public void UpdateNextPrevSessionsBtns()
        {
        // Do smth
        }
```

* On startup create manager with list of targets where functions are situated
```
        Global.HotkeysManager = new HotkeysManager(new object[] { this, GridDeciphereChannel });
        Global.HotkeysManager.Load();
```

* Subscribe MainWindow's OnKeyDown and pass params to manager
```
        if (Global.HotkeysManager.TryRaiseHotkey(e.Key, Keyboard.Modifiers))
        {
            return;
        }
```
* ...
* Done!
