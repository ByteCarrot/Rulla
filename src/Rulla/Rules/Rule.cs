namespace ByteCarrot.Rulla.Rules
{
    public abstract class Rule<TModel>
    {
        public string Text { get; protected set; }

        public abstract bool Apply(TModel model);
    }
}