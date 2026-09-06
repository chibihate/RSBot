namespace RSBot.Controller.Models;

public class AccountEntry
{
    public string Username { get; set; }
    public AccountStatus Status { get; set; } = AccountStatus.Pending;

    public string StatusText => Status switch
    {
        AccountStatus.Pending => "Pending",
        AccountStatus.Running => "Running...",
        AccountStatus.Done => "Done",
        AccountStatus.Error => "Error",
        _ => "-",
    };
}

public enum AccountStatus
{
    Pending,
    Running,
    Done,
    Error,
}
