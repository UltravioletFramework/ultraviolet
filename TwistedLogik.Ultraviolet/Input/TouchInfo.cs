using System;

namespace TwistedLogik.Ultraviolet
{
	/// <summary>
	/// Represents the parameters of an active touch event.
	/// </summary>
	public struct TouchInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TouchInfo"/> structure.
		/// </summary>
		/// <param name="fingerID">The identifier of the finger which caused the touch event.</param>
		/// <param name="x">The normalized x-coordinate of the touch.</param>
		/// <param name="y">The normalized y-coordinate of the touch.</param>
		/// <param name="pressure">The normalized pressure of the touch.</param>
		public TouchInfo(Int64 fingerID, Single x, Single y, Single pressure)
		{
			this.fingerID = fingerID;
			this.x = x;
			this.y = y;
			this.pressure = pressure;
		}

		/// <summary>
		/// Gets the identifier of the finger which caused the touch event.
		/// </summary>
		public Int64 FingerID => fingerID;

		/// <summary>
		/// Gets the normalized x-coordinate of the touch.
		/// </summary>
		public Single X => x;

		/// <summary>
		/// Gets the normalized y-coordinate of the touch.
		/// </summary>
		public Single Y => y;

		/// <summary>
		/// Gets the normalized pressure of the touch.
		/// </summary>
		public Single Pressure => pressure;

		// Property values.
		private readonly Int64 fingerID;
		private readonly Single x;
		private readonly Single y;
		private readonly Single pressure;
	}
}

