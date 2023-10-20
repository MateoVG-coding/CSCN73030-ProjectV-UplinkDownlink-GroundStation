using WebIncludes;
using Threading;
using link;
class MainProgram
{
    static public void Main(String[] args)
    {

        //do something
        LinkController controller = new LinkController();
        controller.CreateEndpoints();
    }
}