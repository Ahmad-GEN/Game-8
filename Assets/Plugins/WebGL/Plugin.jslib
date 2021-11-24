mergeInto(LibraryManager.library, {
_IsMobile: function()
     {
        return gameInstance.Module.SystemInfo.mobile;
     },

  _OnGameStarted: function () {
    OnGameStarted();
  },
  
 _OnGameStopped: function () {
    OnGameStopped();
  },
  
 _ExitFullScreen: function () {
    ExitFullScreen();
  },
  
});