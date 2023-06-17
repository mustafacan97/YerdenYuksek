namespace YerdenYuksek.Core.Primitives;

public interface ISoftDeletedEntity
{
    bool Deleted { get; set; }
}
