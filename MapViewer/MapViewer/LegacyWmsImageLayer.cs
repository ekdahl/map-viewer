using MapControl;

namespace MapViewer
{
	internal partial class LegacyWmsImageLayer : WmsImageLayer
	{
		protected override string GetMapRequestUri(BoundingBox boundingBox)
		{
			return base.GetMapRequestUri(boundingBox).Replace("CRS=", "SRS=");
		}
	}
}
