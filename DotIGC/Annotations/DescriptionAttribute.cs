namespace DotIGC.Annotations
{
    using System;

    internal class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(string shortDescription, string detailedDescription)
        {
            Short = shortDescription;
            Detailed = detailedDescription;
        }

        public DescriptionAttribute(string shortDescription) : this(shortDescription, string.Empty) { }

        public string Short { get; private set; }

        public string Detailed { get; private set; }
    }
}
