﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using ClassicalSharp;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform.Windows;
using ClassicalSharp.Network;

namespace Launcher2 {

	public sealed class LauncherWindow {
		
		public NativeWindow Window;
		public IDrawer2D Drawer;
		public LauncherScreen screen;
		public bool Dirty;
		public ClassicubeSession Session = new ClassicubeSession();
		public AsyncDownloader Downloader;
		
		public int Width { get { return Window.Width; } }
		
		public int Height { get { return Window.Height; } }
		
		public Bitmap Framebuffer;
		Font logoFont, logoItalicFont;
		public void Init() {
			Window.Resize += Resize;
			Window.FocusedChanged += FocusedChanged;
			Window.WindowStateChanged += Resize;
			logoFont = new Font( "Times New Roman", 28, FontStyle.Bold );
			logoItalicFont = new Font( "Times New Roman", 28, FontStyle.Italic );
		}

		void FocusedChanged( object sender, EventArgs e ) {
			MakeBackground();
			screen.Resize();
		}

		void Resize( object sender, EventArgs e ) {
			if( screenGraphics != null )
				screenGraphics.Dispose();
			
			screenGraphics = Graphics.FromHwnd( Window.WindowInfo.WinHandle );
			MakeBackground();
			screen.Resize();
		}
		
		public void SetScreen( LauncherScreen screen ) {
			if( this.screen != null )
				this.screen.Dispose();
			
			MakeBackground();
			this.screen = screen;
			screen.Init();
		}
		
		public void Run() {
			Window = new NativeWindow( 480, 480, Program.AppName, 0,
			                          GraphicsMode.Default, DisplayDevice.Default );
			Window.Visible = true;
			Drawer = new GdiPlusDrawer2D( null );
			Init();
			screenGraphics = Graphics.FromHwnd( Window.WindowInfo.WinHandle );
			
			if( !ResourceFetcher.CheckAllResourcesExist() ) {
				SetScreen( new ResourcesScreen( this ) );
			} else {
				SetScreen( new MainScreen( this ) );
			}		
			
			while( true ) {
				Window.ProcessEvents();
				if( !Window.Exists ) break;
				
				screen.Tick();
				if( Dirty || screen.Dirty )
					Display();
				Thread.Sleep( 1 );
			}
		}
		
		Graphics screenGraphics;
		void Display() {
			Dirty = false;
			screen.Dirty = false;
			screenGraphics.DrawImage( Framebuffer, 0, 0, Framebuffer.Width, Framebuffer.Height );
			Window.Invalidate();
		}
		
		internal static FastColour clearColour = new FastColour( 30, 30, 30 );
		public void MakeBackground() {
			if( Framebuffer != null )
				Framebuffer.Dispose();
			
			Framebuffer = new Bitmap( Width, Height );
			using( IDrawer2D drawer = Drawer ) {
				drawer.SetBitmap( Framebuffer );
				drawer.Clear( clearColour );
				
				DrawTextArgs args1 = new DrawTextArgs( "&eClassical", logoItalicFont, true );
				Size size1 = drawer.MeasureSize( ref args1 );
				DrawTextArgs args2 = new DrawTextArgs( "&eSharp", logoFont, true );
				Size size2 = drawer.MeasureSize( ref args2 );
					
				int xStart = Width / 2 - (size1.Width + size2.Width ) / 2;				
				drawer.DrawText( ref args1, xStart, 20 );
				drawer.DrawText( ref args2, xStart + size1.Width, 20 );
			}
			Dirty = true;
		}
		
		public void Dispose() {
			logoFont.Dispose();
			logoItalicFont.Dispose();
		}
	}
}
