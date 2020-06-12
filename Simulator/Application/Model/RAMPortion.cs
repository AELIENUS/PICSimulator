using Applicator.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public  class RAMPortion : ObservableObject
    {
		private AbstractByte _Byte0;

		public AbstractByte Byte0
		{
			get { return _Byte0; }
			set { _Byte0 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte1;

		public AbstractByte Byte1
		{
			get { return _Byte1; }
			set { _Byte1 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte2;

		public AbstractByte Byte2
		{
			get { return _Byte2; }
			set { _Byte2 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte3;

		public AbstractByte Byte3
		{
			get { return _Byte3; }
			set { _Byte3 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte4;

		public AbstractByte Byte4
		{
			get { return _Byte4; }
			set { _Byte4 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte5;

		public AbstractByte Byte5
		{
			get { return _Byte5; }
			set { _Byte5 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte6;

		public AbstractByte Byte6
		{
			get { return _Byte6; }
			set { _Byte6 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte7;

		public AbstractByte Byte7
		{
			get { return _Byte7; }
			set { _Byte7 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte8;

		public AbstractByte Byte8
		{
			get { return _Byte8; }
			set { _Byte8 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte9;

		public AbstractByte Byte9
		{
			get { return _Byte9; }
			set { _Byte9 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte10;

		public AbstractByte Byte10
		{
			get { return _Byte10; }
			set { _Byte10 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte11;

		public AbstractByte Byte11
		{
			get { return _Byte11; }
			set { _Byte11 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte12;

		public AbstractByte Byte12
		{
			get { return _Byte12; }
			set { _Byte12 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte13;

		public AbstractByte Byte13
		{
			get { return _Byte13; }
			set { _Byte13 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte14;

		public AbstractByte Byte14
		{
			get { return _Byte14; }
			set { _Byte14 = value; RaisePropertyChanged(); }
		}

		private AbstractByte _Byte15;

		public AbstractByte Byte15
		{
			get { return _Byte15; }
			set { _Byte15 = value; RaisePropertyChanged(); }
		}

		public RAMPortion()
		{
			Byte0 = new ItemNotifyByte();
			Byte1 = new ItemNotifyByte();
			Byte2 = new ItemNotifyByte();
			Byte3 = new ItemNotifyByte();
			Byte4 = new ItemNotifyByte();
			Byte5 = new ItemNotifyByte();
			Byte6 = new ItemNotifyByte();
			Byte7 = new ItemNotifyByte();
			Byte8 = new ItemNotifyByte();
			Byte9 = new ItemNotifyByte();
			Byte10 = new ItemNotifyByte();
			Byte11 = new ItemNotifyByte();
			Byte12 = new ItemNotifyByte();
			Byte13 = new ItemNotifyByte();
			Byte14 = new ItemNotifyByte();
			Byte15 = new ItemNotifyByte();

		}

	}
}
