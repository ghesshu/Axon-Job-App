namespace Axon_Job_App.Helpers;

public interface IHasTimestamps
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}