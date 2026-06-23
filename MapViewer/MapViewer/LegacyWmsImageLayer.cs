using MapControl;
using System;

namespace MapViewer
{
	internal partial class LegacyWmsImageLayer : WmsImageLayer
	{
		protected override Uri GetMapRequestUri(Rect bbox)
		{
			string uriString = base.GetMapRequestUri(bbox).AbsoluteUri;
			return new Uri(uriString.Replace("CRS=", "SRS="));
		}
	}
}
