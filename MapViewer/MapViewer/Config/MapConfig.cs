using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MapViewer.Config
{
	internal class MapConfig
	{
		public List<LayerBase> Layers { get; set; } = new();
	}

	[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
	[JsonDerivedType(typeof(WmsLayer), "WMS")]
	[JsonDerivedType(typeof(WmtsLayer), "WMTS")]
	[JsonDerivedType(typeof(XyzLayer), "XYZ")]
	[JsonDerivedType(typeof(XyzLayer), "TMS")]

	public abstract class LayerBase
	{
		public string Name { get; set; } = string.Empty;
		public double Opacity { get; set; } = 100.0;
		public bool IsVisible { get; set; } = true;
	}

	// WMS-specific
	public class WmsLayer : LayerBase
	{
		public string ServiceUri { get; set; } = string.Empty;
		public string Layers { get; set; } = string.Empty;
	}

	// WMTS-specific
	public class WmtsLayer : LayerBase
	{
		public string CapabilitiesUri { get; set; } = string.Empty;
		public string Layer { get; set; } = string.Empty;
		public string TileMatrixSet { get; set; } = string.Empty;
		public string Format { get; set; } = "image/png";
	}

	// XYZ tile server
	public class XyzLayer : LayerBase
	{
		public string UriTemplate { get; set; } = string.Empty; // e.g. https://.../{z}/{x}/{y}.png
	}

	// Tile map server
	public class TmsLayer : LayerBase
	{
		public string UriTemplate { get; set; } = string.Empty; // e.g. https://.../{z}/{x}/{y}.png
	}
}
