
public class Managers : SingletonBehavior<Managers>
{
    #region Manager Variables

    private readonly ResourceManager _resourceManager = new();
    private readonly GameManager _gameManager = new();
    private readonly DataManager _dataManager = new();
    private readonly UIManager _uiManager = new();
    private readonly SoundManager _soundManager = new();
    //private readonly SoundManager _soundManager = new();

    #endregion



    #region Manager Properties

    public static ResourceManager Resource => Instance._resourceManager;
    public static GameManager Game => Instance._gameManager;
    public static DataManager Data => Instance._dataManager;
    public static UIManager UI => Instance._uiManager;
    //public static SoundManager Sound => Instance._soundManager;
    public static SoundManager Sound => Instance._soundManager;
    #endregion
}
