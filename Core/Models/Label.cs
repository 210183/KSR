namespace Core.Models
{
    public struct Label
    {
        public Label(string name) : this()
        {
            Name = name;
        }

        public string Name { get; }
    }
}
