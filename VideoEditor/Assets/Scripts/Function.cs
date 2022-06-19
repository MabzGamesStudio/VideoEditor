using System;
using UnityEngine;

/// <summary>
/// A path created by a y output as a function of x
/// </summary>
public abstract class Function : Path
{

	/// <summary>
	/// Starting x value
	/// </summary>
	protected float xStart;

	/// <summary>
	/// Ending x value
	/// </summary>
	protected float xEnd;

	/// <summary>
	/// Vertical shift of function upward
	/// </summary>
	protected float verticalShift;

	/// <summary>
	/// Horizontal shift of function right
	/// </summary>
	protected float horizontalShift;

	/// <summary>
	/// Vertical stretch of function from x-axis
	/// </summary>
	protected float verticalStretch;

	/// <summary>
	/// Horizontal stretch of function from y-axis
	/// </summary>
	protected float horizontalStretch;

	/// <summary>
	/// Default function with default transformation values
	/// </summary>
	protected Function()
	{
		xStart = 0;
		xEnd = 1;
		verticalShift = 0;
		horizontalShift = 0;
		verticalStretch = 1;
		horizontalStretch = 1;
	}

	/// <summary>
	/// Performs transformations of function
	/// </summary>
	/// <param name="function">Function to make transformations</param>
	/// <returns>The transformed function</returns>
	protected Func<float, float> FunctionTransformations(Func<float, float> function)
	{
		return x => verticalStretch * function((x - horizontalShift) / horizontalStretch) + verticalShift;
	}

	/// <summary>
	/// Sets the vertical shift
	/// </summary>
	/// <param name="verticalShift">New vertical shift</param>
	/// <returns>The same function with the vertical shift altered</returns>
	public Function VerticalShift(float verticalShift)
	{
		this.verticalShift = verticalShift;
		return this;
	}

	/// <summary>
	/// Sets the horizontal shift
	/// </summary>
	/// <param name="horizontalShift">New horizontal shift</param>
	/// <returns>The same function with the horizontal shift altered</returns>
	public Function HorizontalShift(float horizontalShift)
	{
		this.horizontalShift = horizontalShift;
		return this;
	}

	/// <summary>
	/// Sets the vertical stretch
	/// </summary>
	/// <param name="verticalStretch">New vertical stretch</param>
	/// <returns>The same function with the vertical stretch altered</returns>
	public Function VerticalStretch(float verticalStretch)
	{
		this.verticalStretch = verticalStretch;
		return this;
	}

	/// <summary>
	/// Sets the horizontal stretch
	/// </summary>
	/// <param name="horizontalStretch">New horizontal stretch</param>
	/// <returns>The same function with the horizontal stretch altered</returns>
	public Function HorizontalStretch(float horizontalStretch)
	{
		this.horizontalStretch = horizontalStretch;
		return this;
	}

	/// <summary>
	/// Sets the x start
	/// </summary>
	/// <param name="xStart">New x start</param>
	/// <returns>The same function with the x start altered</returns>
	public Function XStart(float xStart)
	{
		this.xStart = xStart;
		return this;
	}

	/// <summary>
	/// Sets the x end
	/// </summary>
	/// <param name="xEnd">New x end</param>
	/// <returns>The same function with the x end altered</returns>
	public Function XEnd(float xEnd)
	{
		this.xEnd = xEnd;
		return this;
	}

	/// <summary>
	/// Gets the Function PathType enum
	/// </summary>
	/// <returns>Function PathType enum</returns>
	public PathType GetMovementType()
	{
		return PathType.Function;
	}

	/// <summary>
	/// Whether the given value is in the domain of the function
	/// </summary>
	/// <param name="x">Value to test</param>
	/// <returns>Whether the value is within the domain of the function</returns>
	public abstract bool InDomain(float x);

	/// <summary>
	/// Gets the y value of the function at with the given x position
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>y value</returns>
	public abstract float GetY(float x);

	/// <summary>
	/// Gets the path position with the given progression from 0 to 1
	/// </summary>
	/// <param name="percent">Progression of the path from 0 to 1</param>
	/// <returns>Position on the path</returns>
	public Vector2 GetPosition(float percent)
	{

		// The x value is linearly interpretted from the start to the end x value
		float x = xStart * (1 - percent) + xEnd * percent;

		// If the x value is within the domain of the function, the y value is calculated
		if (InDomain(x))
		{
			return new Vector2(x, GetY(x));
		}

		// If the input is outside the domain the origin is returned to prevent an error
		return new Vector2(0, 0);
	}

}

/// <summary>
/// Sine function
/// </summary>
public class SinFunc : Function
{

	/// <summary>
	/// Creates a sin function
	/// </summary>
	public SinFunc() : base() { }

	/// <summary>
	/// Gets the y position from the x value
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>y value</returns>
	public override float GetY(float x)
	{
		return FunctionTransformations(Mathf.Sin)(x);
	}

	/// <summary>
	/// Sin function has total domain
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>true</returns>
	public override bool InDomain(float x)
	{
		return true;
	}
}

/// <summary>
/// Cosine function
/// </summary>
public class CosFunc : Function
{

	/// <summary>
	/// Creates a cos function
	/// </summary>
	public CosFunc() : base() { }

	/// <summary>
	/// Gets the y position from the x value
	/// </summary>
	/// <param name="x">x value</param>
	/// <returnsy value></returns>
	public override float GetY(float x)
	{
		return FunctionTransformations(Mathf.Cos)(x);
	}

	/// <summary>
	/// Cos function has total domain
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>true</returns>
	public override bool InDomain(float x)
	{
		return true;
	}
}

/// <summary>
/// Tangent function
/// </summary>
public class TanFunc : Function
{

	/// <summary>
	/// Creates a tan function
	/// </summary>
	public TanFunc() : base() { }

	/// <summary>
	/// Gets the y value from the x value
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>y value</returns>
	public override float GetY(float x)
	{
		return FunctionTransformations(Mathf.Tan)(x);
	}

	/// <summary>
	/// Tan function is undefined when cos(x) = 0
	/// (with transformations is a bit different)
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>Whether function is defined at x</returns>
	public override bool InDomain(float x)
	{
		return Mathf.Cos((x - horizontalShift) / horizontalStretch) != 0f;
	}
}

/// <summary>
/// Cosecant function
/// </summary>
public class CscFunc : Function
{

	/// <summary>
	/// Creates a csc function
	/// </summary>
	public CscFunc() : base() { }

	/// <summary>
	/// Gets the y value from the x value
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>y value</returns>
	public override float GetY(float x)
	{
		return FunctionTransformations(x => 1 / Mathf.Sin(x))(x);
	}

	/// <summary>
	/// Csc function is undefined when sin(x) = 0
	/// (with transformations is a bit different)
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>Whether function is defined at x</returns>
	public override bool InDomain(float x)
	{
		return Mathf.Sin((x - horizontalShift) / horizontalStretch) != 0;
	}
}

/// <summary>
/// Secant function
/// </summary>
public class SecFunc : Function
{
	/// <summary>
	/// Creates a sec function
	/// </summary>
	public SecFunc() : base() { }

	/// <summary>
	/// Gets the y value from the x value
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>y value</returns>
	public override float GetY(float x)
	{
		return FunctionTransformations(x => 1 / Mathf.Cos(x))(x);
	}

	/// <summary>
	/// Sec function is undefined when cos(x) = 0
	/// (with transformations is a bit different)
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>Whether function is defined at x</returns>
	public override bool InDomain(float x)
	{
		return Mathf.Cos((x - horizontalShift) / horizontalStretch) != 0;
	}
}

/// <summary>
/// Cotangent function
/// </summary>
public class CotFunc : Function
{
	/// <summary>
	/// Creates a cot function
	/// </summary>
	public CotFunc() : base() { }

	/// <summary>
	/// Gets the y value from the x value
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>y value</returns>
	public override float GetY(float x)
	{
		return FunctionTransformations(x => Mathf.Cos(x) / Mathf.Sin(x))(x);
	}

	/// <summary>
	/// Cot function is undefined when sin(x) = 0
	/// (with transformations is a bit different)
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>Whether the funciton is defined at x</returns>
	public override bool InDomain(float x)
	{
		return Mathf.Sin((x - horizontalShift) / horizontalStretch) != 0f;
	}
}

/// <summary>
/// Quadratic function
/// </summary>
public class QuadraticFunc : Function
{
	/// <summary>
	/// Creates a quadratic function
	/// </summary>
	public QuadraticFunc() : base() { }

	/// <summary>
	/// Creates a quadratic function given the a, b, and c values
	/// </summary>
	public QuadraticFunc(float a, float b, float c) : base()
	{
		horizontalShift = -b / (2 * a);
		verticalShift = a * Mathf.Pow(horizontalShift, 2) + b * horizontalShift + c;
		verticalStretch = a;
		horizontalStretch = 1f;
	}

	/// <summary>
	/// Gets the y value from the x value
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>y value</returns>
	public override float GetY(float x)
	{

		return FunctionTransformations(x => Mathf.Pow(x, 2))(x);
	}

	/// <summary>
	/// Quadratic function is total
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>true</returns>
	public override bool InDomain(float x)
	{
		return true;
	}
}

/// <summary>
/// Square root function
/// </summary>
public class SqrtFunc : Function
{
	/// <summary>
	/// Creates a sqrt function
	/// </summary>
	public SqrtFunc() : base() { }

	/// <summary>
	/// Gets the y value from the x value
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>y value</returns>
	public override float GetY(float x)
	{

		return FunctionTransformations(Mathf.Sqrt)(x);
	}

	/// <summary>
	/// Sqrt function is defined when the x value is non-negative
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>Whether function is defined at x</returns>
	public override bool InDomain(float x)
	{
		return (x - horizontalShift) / horizontalStretch >= 0;
	}
}

/// <summary>
/// Polynomial function
/// </summary>
public class PolynomialFunc : Function
{

	/// <summary>
	/// The degree of the function
	/// </summary>
	private int degree;

	/// <summary>
	/// The list of coefficients from the constant to the highest degree term
	/// </summary>
	private float[] coefficients;

	/// <summary>
	/// Creates a plain polynomial (ie y = x^4, with degree = 4)
	/// </summary>
	public PolynomialFunc(int degree) : base()
	{
		this.degree = degree;
		coefficients = new float[degree + 1];
		coefficients[degree] = 1;
	}

	/// <summary>
	/// Creates a polynomial with the given coefficients
	/// </summary>
	public PolynomialFunc(float[] coefficients) : base()
	{
		this.degree = coefficients.Length;
		this.coefficients = coefficients;
	}

	/// <summary>
	/// Gets the y value from the x value
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>y value</returns>
	public override float GetY(float x)
	{

		// Polynomial function
		Func<float, float> polynomial = delegate (float x)
		{

			// Sums the coefficient times the x^degree for each term
			float sum = 0;
			for (int i = degree; i >= 0; i++)
			{
				if (i == 0)
				{
					sum += coefficients[coefficients.Length - 1];
				}
				else
				{
					sum += coefficients[i] * Mathf.Pow(x, i);
				}
			}
			return sum;
		};

		// Returns the y value of the polynomial function with the given x value
		return polynomial(x);
	}

	/// <summary>
	/// Polynomial function has total domain
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>true</returns>
	public override bool InDomain(float x)
	{
		return true;
	}
}

/// <summary>
/// Exponential function
/// </summary>
public class ExponentialFunc : Function
{

	/// <summary>
	/// The base number of the exponential
	/// </summary>
	private float baseNumber;

	/// <summary>
	/// Mathematical constant e
	/// </summary>
	private static float e = 2.7182818284590f;

	/// <summary>
	/// Creates an exponential function with base e
	/// </summary>
	public ExponentialFunc() : base() { baseNumber = e; }

	/// <summary>
	/// Creates an exponential function with base baseNumber
	/// </summary>
	public ExponentialFunc(float baseNumber) : base() { this.baseNumber = baseNumber; }

	/// <summary>
	/// Gets the y value from the x value
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>y value</returns>
	public override float GetY(float x)
	{
		return FunctionTransformations(x => Mathf.Pow(baseNumber, x))(x);
	}

	/// <summary>
	/// Exponential function has total domain
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>true</returns>
	public override bool InDomain(float x)
	{
		return true;
	}
}

/// <summary>
/// Logarithm function
/// </summary>
public class LogFunc : Function
{

	/// <summary>
	/// The base of the log function
	/// </summary>
	private float baseNumber;

	/// <summary>
	/// Mathematical constant e
	/// </summary>
	private static float e = 2.7182818284590f;

	/// <summary>
	/// Creates a log function with base e
	/// </summary>
	public LogFunc() : base() { baseNumber = e; }

	/// <summary>
	/// Creates a log function with base baseNumber
	/// </summary>
	public LogFunc(float baseNumber) : base() { this.baseNumber = baseNumber; }

	/// <summary>
	/// Gets the y value from the x value
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>y value</returns>
	public override float GetY(float x)
	{
		return FunctionTransformations(x => Mathf.Log(x, baseNumber))(x);
	}

	/// <summary>
	/// log function is defined when the x value is greater than 0
	/// (with transformations is a bit different)
	/// </summary>
	/// <param name="x">x value</param>
	/// <returns>true</returns>
	public override bool InDomain(float x)
	{
		return (x - horizontalShift) / horizontalStretch > 0f;
	}
}
