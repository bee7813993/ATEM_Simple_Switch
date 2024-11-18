﻿/* -LICENSE-START-
** Copyright (c) 2018 Blackmagic Design
**
** Permission is hereby granted, free of charge, to any person or organization
** obtaining a copy of the software and accompanying documentation covered by
** this license (the "Software") to use, reproduce, display, distribute,
** execute, and transmit the Software, and to prepare derivative works of the
** Software, and to permit third-parties to whom the Software is furnished to
** do so, all subject to the following:
** 
** The copyright notices in the Software and this entire statement, including
** the above license grant, this restriction and the following disclaimer,
** must be included in all copies of the Software, in whole or in part, and
** all derivative works of the Software, unless such copies or derivative
** works are solely in the form of machine-executable object code generated by
** a source language processor.
** 
** THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
** IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
** FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
** SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
** FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
** ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
** DEALINGS IN THE SOFTWARE.
** -LICENSE-END-
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using BMDSwitcherAPI;

namespace SimpleSwitcherExampleCSharp
{
	class Program
	{
		static long GetInputId(IBMDSwitcherInput input)
		{
			long id;
			input.GetInputId(out id);
			return id;
		}

		static void Main(string[] args)
		{
			// Create switcher discovery object
			IBMDSwitcherDiscovery discovery = new CBMDSwitcherDiscovery();

			// Connect to switcher
			IBMDSwitcher switcher;
			_BMDSwitcherConnectToFailure failureReason;
			discovery.ConnectTo("192.168.10.240", out switcher, out failureReason);
			Console.WriteLine("Connected to switcher");

			var atem = new AtemSwitcher(switcher);

			// Get reference to various objects
			IBMDSwitcherMixEffectBlock me0 = atem.MixEffectBlocks.First();
			IBMDSwitcherTransitionParameters me0TransitionParams = me0 as IBMDSwitcherTransitionParameters;
			IBMDSwitcherTransitionWipeParameters me0WipeTransitionParams = me0 as IBMDSwitcherTransitionWipeParameters;
			IBMDSwitcherInput input4 = atem.SwitcherInputs
										.Where((i, ret) => {
											_BMDSwitcherPortType type;
											i.GetPortType(out type);
											return type == _BMDSwitcherPortType.bmdSwitcherPortTypeExternal;
										})
										.ElementAt(4);
			// Setup the transition
			Console.WriteLine("Setting preview input");
			me0.SetPreviewInput(GetInputId(input4));

			Console.WriteLine("Setting next transition selection");
			me0TransitionParams.SetNextTransitionSelection(_BMDSwitcherTransitionSelection.bmdSwitcherTransitionSelectionBackground);

			Console.WriteLine("Setting next transition style");
			me0TransitionParams.SetNextTransitionStyle(_BMDSwitcherTransitionStyle.bmdSwitcherTransitionStyleWipe);

			Console.WriteLine("Setting transition style");
			me0WipeTransitionParams.SetPattern(_BMDSwitcherPatternStyle.bmdSwitcherPatternStyleRectangleIris);

			Console.WriteLine("Setting transition rate");
			me0WipeTransitionParams.SetRate(60);
			
			// Perform the transition
			Console.WriteLine("Performing auto transition");
			me0.PerformAutoTransition();
			System.Threading.Thread.Sleep(2000);
			System.Threading.Thread.Sleep(1000);
			me0.PerformAutoTransition();

			Console.Write("Press ENTER to exit...");
			Console.ReadLine();
		}
	}

	internal class AtemSwitcher
	{
		private IBMDSwitcher switcher;

		public AtemSwitcher(IBMDSwitcher switcher)
		{
			this.switcher = switcher;
		}

		public IEnumerable<IBMDSwitcherMixEffectBlock> MixEffectBlocks
		{
			get
			{
				// Create a mix effect block iterator
				IntPtr meIteratorPtr;
				switcher.CreateIterator(typeof(IBMDSwitcherMixEffectBlockIterator).GUID, out meIteratorPtr);
				IBMDSwitcherMixEffectBlockIterator meIterator = Marshal.GetObjectForIUnknown(meIteratorPtr) as IBMDSwitcherMixEffectBlockIterator;
				if (meIterator == null)
					yield break;

				// Iterate through all mix effect blocks
				while (true)
				{
					IBMDSwitcherMixEffectBlock me;
					meIterator.Next(out me);

					if (me != null)
						yield return me;
					else
						yield break;
				}
			}
		}

		public IEnumerable<IBMDSwitcherInput> SwitcherInputs
		{
			get
			{
				// Create an input iterator
				IntPtr inputIteratorPtr;
				switcher.CreateIterator(typeof(IBMDSwitcherInputIterator).GUID, out inputIteratorPtr);
				IBMDSwitcherInputIterator inputIterator = Marshal.GetObjectForIUnknown(inputIteratorPtr) as IBMDSwitcherInputIterator;
				if (inputIterator == null)
					yield break;

				// Scan through all inputs
				while (true)
				{
					IBMDSwitcherInput input;
					inputIterator.Next(out input);

					if (input != null)
						yield return input;
					else
						yield break;
				}
			}
		}
	}
}
