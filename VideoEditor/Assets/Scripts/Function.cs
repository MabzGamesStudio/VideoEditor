using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Function : Path
{

	protected float xStart;

	protected float xEnd;

	protected float verticalShift;

	protected float horizontalShift;

	protected float verticalStretch;

	protected float horizontalStretch;

	protected Function()
	{
		xStart = 0;
		xEnd = 1;
		verticalShift = 0;
		horizontalShift = 0;
		verticalStretch = 1;
		horizontalStretch = 1;
	}

	protected Func<float, float> FunctionTransformations(Func<float, float> function)
	{
		return x => verticalStretch * function((x - horizontalShift) / horizontalStretch) + verticalShift;
	}

	public Function VerticalShift(float verticalShift)
	{
		this.verticalShift = verticalShift;
		return this;
	}

	public Function HorizontalShift(float horizontalShift)
	{
		this.horizontalShift = horizontalShift;
		return this;
	}

	public Function VerticalStretch(float verticalStretch)
	{
		this.verticalStretch = verticalStretch;
		return this;
	}

	public Function HorizontalStretch(float horizontalStretch)
	{
		this.horizontalStretch = horizontalStretch;
		return this;
	}

	public Function XStart(float xStart)
	{
		this.xStart = xStart;
		return this;
	}

	public Function XEnd(float xEnd)
	{
		this.xEnd = xEnd;
		return this;
	}

	public PathType GetMovementType()
	{
		return PathType.Function;
	}

	public abstract bool InDomain(float x);

	public abstract float GetY(float x);

	public Vector2 GetPosition(float percent)
	{
		float x = xStart * (1 - percent) + xEnd * percent;
		if (InDomain(x))
		{
			return new Vector2(x, GetY(x));
		}
		return new Vector2(0, 0);

	}

}

public class SinFunc : Function
{

	public SinFunc() : base() { }

	public override float GetY(float x)
	{
		return FunctionTransformations(Mathf.Sin)(x);
	}

	public override bool InDomain(float x)
	{
		return true;
	}
}

public class CosFunc : Function
{
	public CosFunc() : base() { }

	public override float GetY(float x)
	{
		return FunctionTransformations(Mathf.Cos)(x);
	}

	public override bool InDomain(float x)
	{
		return true;
	}
}

public class TanFunc : Function
{
	public TanFunc() : base() { }

	public override float GetY(float x)
	{
		return FunctionTransformations(Mathf.Tan)(x);
	}

	public override bool InDomain(float x)
	{
		return Mathf.Cos((x - horizontalShift) / horizontalStretch) != 0f;
	}
}

public class CscFunc : Function
{

	public CscFunc() : base() { }

	public override float GetY(float x)
	{
		return FunctionTransformations(x => 1 / Mathf.Sin(x))(x);
	}

	public override bool InDomain(float x)
	{
		return Mathf.Sin((x - horizontalShift) / horizontalStretch) != 0;
	}
}

public class SecFunc : Function
{
	public SecFunc() : base() { }

	public override float GetY(float x)
	{
		return FunctionTransformations(x => 1 / Mathf.Cos(x))(x);
	}

	public override bool InDomain(float x)
	{
		return Mathf.Sin((x - horizontalShift) / horizontalStretch) != 0;
	}
}

public class CotFunc : Function
{
	public CotFunc() : base() { }

	public override float GetY(float x)
	{
		return FunctionTransformations(x => Mathf.Cos(x) / Mathf.Sin(x))(x);
	}

	public override bool InDomain(float x)
	{
		return Mathf.Sin((x - horizontalShift) / horizontalStretch) != 0f;
	}
}

public class QuadraticFunc : Function
{
	public QuadraticFunc() : base() { }

	public QuadraticFunc(float a, float b, float c) : base()
	{
		horizontalShift = -b / (2 * a);
		verticalShift = a * Mathf.Pow(horizontalShift, 2) + b * horizontalShift + c;
		verticalStretch = a;
		horizontalStretch = 1f;
	}

	public override float GetY(float x)
	{

		return FunctionTransformations(x => Mathf.Pow(x, 2))(x);
	}

	public override bool InDomain(float x)
	{
		return true;
	}
}

public class SqrtFunc : Function
{
	public SqrtFunc() : base() { }

	public override float GetY(float x)
	{

		return FunctionTransformations(Mathf.Sqrt)(x);
	}

	public override bool InDomain(float x)
	{
		return (x - horizontalShift) / horizontalStretch >= 0;
	}
}

public class PolynomialFunc : Function
{
	private int power;

	private float[] coefficients;

	public PolynomialFunc(int power) : base()
	{
		this.power = power;
		coefficients = new float[] { 1f };
	}

	public PolynomialFunc(float[] coefficients) : base()
	{
		this.power = coefficients.Length;
		this.coefficients = coefficients;
	}

	public override float GetY(float x)
	{
		Func<float, float> polynomial = delegate (float x)
		{
			float sum = 0;
			for (int i = power; i >= 0; i++)
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
		return polynomial(x);
	}

	public override bool InDomain(float x)
	{
		return true;
	}
}

public class ExponentialFunc : Function
{
	private float baseNumber;

	private static float e = 2.7182818284590f;

	public ExponentialFunc() : base() { baseNumber = e; }

	public ExponentialFunc(float baseNumber) : base() { this.baseNumber = baseNumber; }

	public override float GetY(float x)
	{
		return FunctionTransformations(x => Mathf.Pow(baseNumber, x))(x);
	}

	public override bool InDomain(float x)
	{
		return true;
	}
}

public class LogFunc : Function
{
	private float baseNumber;

	private static float e = 2.7182818284590f;

	public LogFunc() : base() { baseNumber = e; }

	public LogFunc(float baseNumber) : base() { this.baseNumber = baseNumber; }

	public override float GetY(float x)
	{
		return FunctionTransformations(x => Mathf.Log(x, baseNumber))(x);
	}

	public override bool InDomain(float x)
	{
		return (x - horizontalShift) / horizontalStretch > 0f;
	}
}
