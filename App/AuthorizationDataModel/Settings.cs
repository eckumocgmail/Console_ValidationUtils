[EntityLabel("Настройки")]
public partial class Settings : BaseEntity
{

    [Label("Вертикальное позиционирование")]
    public bool VertialLayout { get; set; }

    [Label("Включить поведение не требующее действий со стороны пользователя")]
    public bool FocusControls { get; set; }

    [Label("Цветовая схема")]
    [SelectControl("Light,Dark,Blue,Modern")]
    public string ColorTheme { get; set; }


    [Label("Публиковать мои действия")]
    public bool PublicOperations { get; set; }


    [Label("Передавать сообщения на электронную почту")]
    public bool SendNewsToEmail { get; set; }


    [Label("Показывать справочную информацию в интерактивном режиме")]
    public bool ShowHelp { get; set; }


    [Label("Оценивать мои способности работы с системой")]
    public bool EvaluateMe { get; set; }



}