using System; 

namespace tvn.cosine.ai.logic.common
{
    static class SourceVersion
    {  
        public static int codePointAt(this string value, int index)
        {
            if ((index < 0) || (index >= value.Length))
            {
                throw new IndexOutOfRangeException(index.ToString());
            }
            return Character.codePointAtImpl(value.ToCharArray(), index, value.Length);
        }
         
        public static bool isIdentifier(string name)
        {
            string id = name.ToString();

            if (id.Length  == 0)
            {
                return false;
            }
            int cp = id.codePointAt(0);
            if (!Character.isJavaIdentifierStart(cp))
            {
                return false;
            }
            for (int i = Character.charCount(cp);
                    i < id.Length;
                    i += Character.charCount(cp))
            {
                cp = id.codePointAt(i);
                if (!Character.isJavaIdentifierPart(cp))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
