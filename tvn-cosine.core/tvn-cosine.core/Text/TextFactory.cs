using tvn.cosine.text.api;

namespace tvn.cosine.text
{
    public static class TextFactory
    {  
        public static IStringBuilder CreateStringBuilder()
        {
            return new StringBuilder();
        }

        public static IStringBuilder CreateStringBuilder(string value)
        {
            return new StringBuilder(value);
        }

        public static IRegularExpression CreateRegularExpression(string pattern)
        {
            return new RegularExpression(pattern);
        }
    }
}
