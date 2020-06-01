﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

using Application = Android.App.Application;

[assembly: Dependency(typeof(triaxis.Xamarin.BluetoothLE.Android.Platform))]

[assembly: UsesPermission("android.permission.BLUETOOTH")]
[assembly: UsesPermission("android.permission.BLUETOOTH_ADMIN")]
[assembly: UsesPermission("android.permission.ACCESS_FINE_LOCATION")]
[assembly: UsesPermission("android.permission.ACCESS_COARSE_LOCATION")]

namespace triaxis.Xamarin.BluetoothLE.Android
{
    class Platform : IBluetoothLE
    {
        ReplaySubject<IAdapter> _adapterSubject;
        IAdapter _adapter;

        public IObservable<IAdapter> WhenAdapterChanges()
            => _adapterSubject ?? (_adapterSubject = Init());

        ReplaySubject<IAdapter> Init()
        {
            var subj = new ReplaySubject<IAdapter>(1);
            var appContext = Application.Context;
            var receiver = new StateBroadcastReceiver() { _owner = this };
            var intentFilter = new IntentFilter(BluetoothAdapter.ActionStateChanged);
            appContext.RegisterReceiver(receiver, intentFilter);

            var manager = (BluetoothManager)Application.Context.GetSystemService(Context.BluetoothService);
            var adapter = manager.Adapter;
            subj.OnNext(_adapter = new Adapter(adapter));
            return subj;
        }

        internal class StateBroadcastReceiverImpl : BroadcastReceiver
        {
            internal Platform _owner;

            public override void OnReceive(Context context, Intent intent)
            {
                _owner._adapterSubject.OnNext(_owner._adapter);
            }
        }
    }

    class StateBroadcastReceiver : Platform.StateBroadcastReceiverImpl
    {
    }
}
