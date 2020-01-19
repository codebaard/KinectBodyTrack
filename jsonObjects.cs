using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.BodyBasics
{

    //This class is the JSON Object, which is sent to the server on every frame update

    class jsonObjects
    {
        public jsonObjects()
        {
            CaptureArea = new captureArea();
            Persons = new List<person>();
        }

        public captureArea CaptureArea;
        public List<person> Persons;

    }

    class captureArea
    {
        public int widthColorFrame;
        public int heightColorFrame;

        public int widthPoseData;
        public int heightPoseData;
    }

    class person
    {
        public person(int myindex) {
            index = myindex;
            PoseData = new poseData[25];
        }
        public int index;
        public poseData[] PoseData;
    }

    class poseData
    {
        public int index;
        public int x; //pixel
        public int y; //pixel
        public float z; //metres       
    }

}
