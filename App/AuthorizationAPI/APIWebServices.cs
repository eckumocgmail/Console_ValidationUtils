 
/// <summary>
/// Сервис управления службами веб-API
/// </summary>
public interface APIWebServices : APIActiveCollection<Service >
{

    /// <summary>
    /// Регистрация сервиса в каталоге, возвращает последовательность.
    /// </summary>
    public byte[] Publish(Service service);
    void Authenticate(byte[] privateKey);
}
