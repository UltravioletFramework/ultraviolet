using Hebron.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace StbImageSharp
{
	internal class AnimatedGifEnumerator : IEnumerator<AnimatedFrameResult>
	{
		private readonly StbImage.stbi__context _context;
		private StbImage.stbi__gif _gif;

		public AnimatedGifEnumerator(Stream input, ColorComponents colorComponents)
		{
			if (input == null) throw new ArgumentNullException(nameof(input));

			_context = new StbImage.stbi__context(input);

			if (StbImage.stbi__gif_test(_context) == 0) throw new Exception("Input stream is not GIF file.");

			_gif = new StbImage.stbi__gif();
			ColorComponents = colorComponents;
		}

		public ColorComponents ColorComponents { get; }

		public AnimatedFrameResult Current { get; private set; }

		object IEnumerator.Current => Current;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public unsafe bool MoveNext()
		{
			// Read next frame
			int ccomp;
			byte two_back;
			var result = StbImage.stbi__gif_load_next(_context, _gif, &ccomp, (int)ColorComponents, &two_back);
			if (result == null) return false;

			if (Current == null)
			{
				Current = new AnimatedFrameResult
				{
					Width = _gif.w,
					Height = _gif.h,
					SourceComp = (ColorComponents)ccomp,
					Comp = ColorComponents == ColorComponents.Default ? (ColorComponents)ccomp : ColorComponents
				};

				Current.Data = new byte[Current.Width * Current.Height * (int)Current.Comp];
			}

			Current.DelayInMs = _gif.delay;

			Marshal.Copy(new IntPtr(result), Current.Data, 0, Current.Data.Length);

			return true;
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}

		~AnimatedGifEnumerator()
		{
			Dispose(false);
		}

		protected unsafe virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_gif != null)
				{
					if (_gif._out_ != null)
					{
						CRuntime.free(_gif._out_);
						_gif._out_ = null;
					}

					if (_gif.history != null)
					{
						CRuntime.free(_gif.history);
						_gif.history = null;
					}

					if (_gif.background != null)
					{
						CRuntime.free(_gif.background);
						_gif.background = null;
					}

					_gif = null;
				}
			}
		}
	}

	internal class AnimatedGifEnumerable : IEnumerable<AnimatedFrameResult>
	{
		private readonly Stream _input;

		public AnimatedGifEnumerable(Stream input, ColorComponents colorComponents)
		{
			_input = input;
			ColorComponents = colorComponents;
		}

		public ColorComponents ColorComponents { get; }

		public IEnumerator<AnimatedFrameResult> GetEnumerator()
		{
			return new AnimatedGifEnumerator(_input, ColorComponents);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}