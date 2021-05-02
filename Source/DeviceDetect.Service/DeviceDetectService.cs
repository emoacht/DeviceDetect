using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDetect.Service
{
	public partial class DeviceDetectService : ServiceBase
	{
		public DeviceDetectService()
		{
			InitializeComponent();
		}

		private static readonly Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");

		private IntPtr _notificationHandle;

		protected override void OnStart(string[] args)
		{
			DeviceNotification.Unregister(_notificationHandle);
			_notificationHandle = DeviceNotification.Register(this.ServiceHandle, GUID_DEVINTERFACE_USB_DEVICE);
		}

		protected override void OnStop()
		{
			DeviceNotification.Unregister(_notificationHandle);
			_notificationHandle = IntPtr.Zero;
		}

		protected override void OnCustomCommand(int command)
		{
			switch (command)
			{
				case DeviceNotification.SERVICE_CONTROL_DEVICEEVENT:
					Debug.WriteLine("USB device event received!");
					this.EventLog.WriteEntry("USB device event received!");
					break;
			}
		}
	}

	public static class DeviceNotification
	{
		[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr RegisterDeviceNotification(
			IntPtr hRecipient,
			IntPtr NotificationFilter,
			uint Flags);

		[DllImport("User32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnregisterDeviceNotification(IntPtr Handle);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct DEV_BROADCAST_DEVICEINTERFACE
		{
			public uint dbcc_size;
			public uint dbcc_devicetype;
			public uint dbcc_reserved;
			public Guid dbcc_classguid;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
			public string dbcc_name;
		}

		private const int DEVICE_NOTIFY_SERVICE_HANDLE = 0x00000001;
		private const int DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;

		public const int SERVICE_CONTROL_DEVICEEVENT = 0x0000000B;
		public const int DBT_DEVICEARRIVAL = 0x8000;
		public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

		/// <summary>
		/// Registers to device events.
		/// </summary>
		/// <param name="serviceHandle">Service status handle</param>
		/// <param name="classGuid">Device interface class Guid</param>
		/// <returns>Device notification handle</returns>
		public static IntPtr Register(IntPtr serviceHandle, Guid classGuid)
		{
			var dbcc = new DEV_BROADCAST_DEVICEINTERFACE
			{
				dbcc_size = (uint)Marshal.SizeOf<DEV_BROADCAST_DEVICEINTERFACE>(),
				dbcc_devicetype = DBT_DEVTYP_DEVICEINTERFACE,
				dbcc_classguid = classGuid
			};

			var buffer = IntPtr.Zero;
			try
			{
				buffer = Marshal.AllocHGlobal((int)dbcc.dbcc_size);
				Marshal.StructureToPtr(dbcc, buffer, true);

				return RegisterDeviceNotification(
					serviceHandle,
					buffer,
					DEVICE_NOTIFY_SERVICE_HANDLE);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Failed to register.\r\n{ex}");
				return IntPtr.Zero;
			}
			finally
			{
				if (buffer != IntPtr.Zero)
					Marshal.FreeHGlobal(buffer);
			}
		}

		/// <summary>
		/// Unregisters from device events.
		/// </summary>
		/// <param name="notificationHandle">Device notification handle</param>
		public static void Unregister(IntPtr notificationHandle)
		{
			if (notificationHandle != IntPtr.Zero)
				UnregisterDeviceNotification(notificationHandle);
		}
	}
}