using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn_cosine.languagedetector.test.unit
{
    /**
     * @author Nakatani Shuyo
     *
     */
   [TestClass] public class  LanguageTest
    {

        /**
         * @throws java.lang.Exception
         */
        @Before
        public void setUp() throws Exception
        {
        }

        /**
         * @throws java.lang.Exception
         */
        @After
        public void tearDown() throws Exception
        {
        }

        /**
         * Test method for {@link com.cybozu.labs.langdetect.Language#Language(java.lang.String, double)}.
         */
        [TestMethod]
        public final void testLanguage()
        {
            Language lang = new Language(null, 0);
            assertEquals(lang.lang, null);
            assertEquals(lang.prob, 0.0, 0.0001);
            assertEquals(lang.toString(), "");

            Language lang2 = new Language("en", 1.0);
            assertEquals(lang2.lang, "en");
            assertEquals(lang2.prob, 1.0, 0.0001);
            assertEquals(lang2.toString(), "en:1.0");

        }

    }

}
