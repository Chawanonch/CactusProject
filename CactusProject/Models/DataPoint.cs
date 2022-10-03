using System.Runtime.Serialization;

namespace CactusProject.Models
{
	//DataContract for Serializing Data - required to serve in JSON format
	[DataContract]
	public class DataPoint
    {
		
		public DataPoint(string label, double y, double z)
		{
			Label = label;
			Y = y;
			Z = z;
		}

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "label")]
		public string Label = "";

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "y")]
		public Nullable<double> Y = null;
		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "z")]
		public Nullable<double> Z = null;
	}
}
