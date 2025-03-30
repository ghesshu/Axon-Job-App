namespace axon_final_api.Helpers;

public interface IHasTimestamps
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}