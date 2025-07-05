using MapControl;
using MapViewer.Config;
using Microsoft.UI.Xaml;
using System;
using System.Text.Json;
using Windows.ApplicationModel.DataTransfer;

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

			// Uncomment to get a sample config in the clipboard
			//SerializeConfig(GetSampleConfig());
		}

		private static MapConfig GetSampleConfig()
		{
			MapConfig config = new();

			config.Layers.Add(new XyzLayer
			{
				Name = "Topografisk",
				UriTemplate = @"https://minkarta.lantmateriet.se/map/topowebbcache?layer=topowebb&tilematrixset=3857&Service=WMTS&Request=GetTile&TileMatrix={z}&TileCol={x}&TileRow={y}",
				//UriTemplate = @"https://karta.raa.se/lmtopowebb/1.0.0/topowebb/default/3857/{z}/{y}/{x}.png",
				Opacity = 100.0,
				IsVisible = true
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Nedtonad",
				ServiceUri = "https://minkarta.lantmateriet.se/map/topowebb",
				Layers = "topowebbkartan_nedtonad",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new TmsLayer
			{
				Name = "Hitta",
				UriTemplate = @"https://static.hitta.se/tile/v3/0/{z}/{x}/{y}?v=27032025",
				Opacity = 100.0,
				IsVisible = false
			});

			config.Layers.Add(new XyzLayer
			{
				Name = "OpenStreetMap",
				UriTemplate = @"https://tile.openstreetmap.org/{z}/{x}/{y}.png",
				Opacity = 100.0,
				IsVisible = false
			});

			/*WmtsLayer karta = new()
			{
				Name = "Topografisk karta",
				CapabilitiesUri = @"https://minkarta.lantmateriet.se/map/topowebbcache?&Service=WMTS&Request=GetCapabilities",
				Layer = "topowebb",
				TileMatrixSet = "3857",
				Opacity = 100.0,
				IsVisible = false
			};
			config.Layers.Add(karta);*/

			config.Layers.Add(new WmsLayer
			{
				Name = "Flygbild",
				ServiceUri = "https://minkarta.lantmateriet.se/map/ortofoto",
				Layers = "Ortofoto_0.5,Ortofoto_0.4,Ortofoto_0.25,Ortofoto_0.16",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Flygbild ca 1960",
				ServiceUri = "https://minkarta.lantmateriet.se/map/historiskaortofoto",
				Layers = "OI.Histortho_60",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Flygbild ca 1975",
				ServiceUri = "https://minkarta.lantmateriet.se/map/historiskaortofoto",
				Layers = "OI.Histortho_75",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Sj�kort",
				ServiceUri = "https://karta.raa.se/sjokort",
				Layers = "OgcWmsLayer0",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Lutning",
				ServiceUri = "https://karta.raa.se/lmhojdmodell",
				Layers = "terranglutning",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Skuggning",
				ServiceUri = "https://karta.raa.se/lmhojdmodell",
				Layers = "terrangskuggning",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Fastighetsgr�nser",
				ServiceUri = "https://karta.raa.se/lmfastighet?TRANSPARENT=true",
				Layers = "granser,text",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Sockengr�nser",
				ServiceUri = "https://karta.raa.se/geo/arkreg_v1.0/wms?TRANSPARENT=true",
				Layers = "socken",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Landskapsgr�nser",
				ServiceUri = "https://karta.raa.se/geo/arkreg_v1.0/wms?TRANSPARENT=true",
				Layers = "landskap",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "L�mningar",
				ServiceUri = "https://karta.raa.se/geo/arkreg_v1.0/wms?TRANSPARENT=true",
				Layers = "arkreg_v1.0:publicerade_lamningar_centrumpunkt",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Befolkningst�thet",
				ServiceUri = "https://minkarta.lantmateriet.se/map/scb?TRANSPARENT=true",
				Layers = "PD.StatisticalDistribution.TotalPopulation",
				Opacity = 50.0,
				IsVisible = false,
			});

			return config;
		}

		private string SerializeConfig(MapConfig config)
		{
			return JsonSerializer.Serialize(config);
		}

		private void AddLayers(MapConfig config)
		{
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
							LayerName = layer.Name,
							IsLayerEnabled = layer.IsVisible,
							LayerOpacity = layer.Opacity,
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
							LayerName = layer.Name,
							IsLayerEnabled = layer.IsVisible,
							LayerOpacity = layer.Opacity,
						};

						LayersStackPanel.Children.Add(control);
						break;

					case XyzLayer xyzLayer:
						MapTileLayer mapTileLayer = new()
						{
							TileSource = new TileSource() { UriTemplate = xyzLayer.UriTemplate },
							SourceName = layer.Name,
						};
						
						control = new(mapTileLayer, Map)
						{
							LayerName = layer.Name,
							IsLayerEnabled = layer.IsVisible,
							LayerOpacity = layer.Opacity,

						};
						LayersStackPanel.Children.Add(control);
						break;
					case TmsLayer tmsLayer:
						MapTileLayer tileMapLayer = new()
						{
							TileSource = new TmsTileSource() { UriTemplate = tmsLayer.UriTemplate },
							SourceName = layer.Name,
						};

						control = new(tileMapLayer, Map)
						{
							LayerName = layer.Name,
							IsLayerEnabled = layer.IsVisible,
							LayerOpacity = layer.Opacity,

						};
						LayersStackPanel.Children.Add(control);
						break;

				}
			}
		}
	}
}
