namespace eventify.Infrastructure.Messaging;

public class RabbitMqConfig
{
    public string Host { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string Uri { get; init; }
}