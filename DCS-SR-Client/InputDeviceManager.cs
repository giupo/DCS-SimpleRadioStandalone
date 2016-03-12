﻿using SharpDX.DirectInput;
using SharpDX.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ciribob.DCS.SimpleRadio.Standalone.Client
{
    class InputDeviceManager : IDisposable
    {
        private DirectInput directInput;
        List<Device> inputDevices = new List<Device>();

        public delegate void DetectButton(InputDevice inputDevice);

        public delegate void DetectPTTCallback(bool state);

        private volatile bool detectPTT = false;

        public InputDeviceManager(Window window)
        {
            directInput = new DirectInput();
            var deviceInstances = directInput.GetDevices();

            foreach (var deviceInstance in deviceInstances)
            {
                Device device = null;

                if (deviceInstance.Type == DeviceType.Keyboard)
                {
                    device = new SharpDX.DirectInput.Keyboard(this.directInput);
                }
                else if (deviceInstance.Usage == UsageId.GenericJoystick)
                {
                    device = new Joystick(this.directInput, deviceInstance.ProductGuid);
                }
                else if (deviceInstance.Usage == UsageId.GenericGamepad)
                {
                    device = new Joystick(this.directInput, deviceInstance.ProductGuid);
                }


                if (device != null)
                {
                    System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(window);

                    device.SetCooperativeLevel(helper.Handle, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
                    device.Acquire();

                    inputDevices.Add(device);
                }
            }
        }

        public void AssignButton(DetectButton callback)
        {
            //detect the state of all current buttons
            Task.Run(() =>
            {

                bool[,] initial = new bool[this.inputDevices.Count, 128];

                for (int i = 0; i < this.inputDevices.Count; i++)
                {
                    this.inputDevices[i].Poll();

                    if (this.inputDevices[i] is Joystick)
                    {
                        var state = (this.inputDevices[i] as Joystick).GetCurrentState();

                        for (int j = 0; j < 128; j++)
                        {
                            initial[i, j] = state.Buttons[j];
                        }
                    }
                    else if (this.inputDevices[i] is Keyboard)
                    {
                        var state = (this.inputDevices[i] as Keyboard).GetCurrentState();

                        for (int j = 0; j < 128; j++)
                        {
                            initial[i, j] = state.IsPressed(state.AllKeys[j]);
                        }
                    }

                }

                string device = string.Empty;
                int button = 0;
                Guid deviceGuid = Guid.Empty;
                bool found = false;

                while (!found)
                {
                    Thread.Sleep(100);

                    for (int i = 0; i < this.inputDevices.Count; i++)
                    {
                        this.inputDevices[i].Poll();

                        if (this.inputDevices[i] is Joystick)
                        {
                            var state = (this.inputDevices[i] as Joystick).GetCurrentState();

                            for (int j = 0; j < 128; j++)
                            {
                                if (state.Buttons[j] && !initial[i, j])
                                {
                                    found = true;
                                    button = j;
                                    device = this.inputDevices[i].Information.ProductName;
                                    deviceGuid = this.inputDevices[i].Information.InstanceGuid;
                                    break;
                                }
                            }
                        }
                        else if (this.inputDevices[i] is Keyboard)
                        {
                            var state = (this.inputDevices[i] as Keyboard).GetCurrentState();

                            for (int j = 0; j < 128; j++)
                            {
                                if (state.IsPressed(state.AllKeys[j]) && !initial[i, j])
                                {
                                    found = true;
                                    button = j;
                                    device = this.inputDevices[i].Information.ProductName;
                                    deviceGuid = this.inputDevices[i].Information.InstanceGuid;
                                    break;
                                }
                            }
                        }

                        if (found)
                        {
                            break;
                        }
                    }
                }


                Application.Current.Dispatcher.Invoke(new Action(() =>
                {

                    callback(new InputDevice() { Device = device, Button = button, InstanceGUID = deviceGuid });

                }));
            });
        }

        public void StartDetectPTT(InputDevice device, DetectPTTCallback callback)
        {
            detectPTT = true;
            //detect the state of all current buttons
            Task.Run(() =>
            {
                for (int i = 0; i < this.inputDevices.Count; i++)
                {

                    if (this.inputDevices[i].Information.InstanceGuid.Equals(device.InstanceGUID))
                    {
                        if (this.inputDevices[i] is Joystick)
                        {

                            while (detectPTT)
                            {
                                Thread.Sleep(100);
                                this.inputDevices[i].Poll();
                                var state = (this.inputDevices[i] as Joystick).GetCurrentState();

                                callback(state.Buttons[device.Button]);

                            }

                        }
                        else if (this.inputDevices[i] is Keyboard)
                        {
                            while (detectPTT)
                            {

                                Thread.Sleep(100);
                                this.inputDevices[i].Poll();
                                var state = (this.inputDevices[i] as Keyboard).GetCurrentState();

                                callback(state.IsPressed(state.AllKeys[device.Button]));
                            }
                        }


                        return;
                    }
                }

            });
        }
        private void StopPTT()
        {
            detectPTT = false;
        }


        public void Dispose()
        {
            StopPTT();
            foreach (var device in inputDevices)
            {
                if (device != null)
                {
                    device.Unacquire();
                    device.Dispose();
                }
            }
        }
    }
}