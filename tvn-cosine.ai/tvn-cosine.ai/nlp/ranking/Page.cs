namespace tvn.cosine.ai.nlp.ranking
{
    public class Page
    {

        public double authority;
        public double hub;
        private string location;
        private string content;
        private IQueue<string> linkTo;
        private IQueue<string> linkedFrom;

        public Page(string location)
        {
            authority = 0;
            hub = 0;
            this.location = location;
            this.linkTo = Factory.CreateQueue<string>();
            this.linkedFrom = Factory.CreateQueue<string>();
        }

        public string getLocation()
        {
            return location;
        }

        public string getContent()
        {
            return content;
        }

        public bool setContent(string content)
        {
            this.content = content;
            return true;
        }

        public IQueue<string> getInlinks()
        {
            return linkedFrom;
        }

        public IQueue<string> getOutlinks()
        {
            return linkTo;
        }
    }
}
