using MapControl;
using MapViewer.Config;
using Microsoft.UI.Xaml;
using System;

namespace MapViewer
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			AddLayers(GetSampleConfig());
		}

		private MapConfig GetSampleConfig()
		{
			MapConfig config = new();

			XyzLayer xyzLayer = new()
			{
				Name = "OpenStreetMap",
				UriTemplate = @"https://tile.openstreetmap.org/{z}/{x}/{y}.png",
			};

			WmsLayer layer = new()
			{
				Name = "Fastighetsgränser",
				ServiceUri = "https://karta.raa.se/lmfastighet?TRANSPARENT=true",
				Layers = "granser,text"
			};

			config.Layers.Add(xyzLayer);
			config.Layers.Add(layer);

			return config;
		}

		private void AddLayers(MapConfig config)
		{

			//List<MapTileLayerBase> layers = [];

			foreach (LayerBase layer in config.Layers)
			{
				LayerSettingsControl control;

				switch (layer)
				{
					case WmsLayer wmsLayer:
						WmsImageLayer wmsImageLayer = new()
						{
							ServiceUri = new Uri(wmsLayer.ServiceUri),
							WmsLayers = wmsLayer.Layers
						};
						
						control = new(wmsImageLayer, Map)
						{
							LayerName = layer.Name
						};

						LayersStackPanel.Children.Add(control);
						break;

					case WmtsLayer wmtsLayer:
						WmtsTileLayer wmtsTileLayer = new()
						{
							CapabilitiesUri = new Uri(wmtsLayer.CapabilitiesUri),
							SourceName = layer.Name
						};
						
						control = new(wmtsTileLayer, Map)
						{
							LayerName = layer.Name
						};

						LayersStackPanel.Children.Add(control);
						break;

					case XyzLayer xyzLayer:
						MapTileLayer mapTileLayer = new()
						{
							TileSource = new TileSource() { UriTemplate = xyzLayer.UriTemplate },
							SourceName = layer.Name
						};
						
						control = new(mapTileLayer, Map)
						{
							LayerName = layer.Name
						};
						LayersStackPanel.Children.Add(control);
						break;
				}
			}
		}
	}
}
