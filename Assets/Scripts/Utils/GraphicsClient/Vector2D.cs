using System;

namespace GraphicsClient
{
    public struct Vector2D
    {
        public Vector2D(float x, float y)
        {
            this.m_x = x;
            this.m_y = y;
        }

        public float x
        {
            get
            {
                return this.m_x;
            }
            set
            {
                if (!float.IsNaN(value))
                {
                    this.m_x = value;
                }
            }
        }

        public float y
        {
            get
            {
                return this.m_y;
            }
            set
            {
                if (!float.IsNaN(value))
                {
                    this.m_y = value;
                }
            }
        }

        public bool IsZero
        {
            get
            {
                return this.LengthSq < float.MinValue;
            }
        }

        public float LengthSq
        {
            get
            {
                return this.m_x * this.m_x + this.m_y * this.m_y;
            }
        }

        public float Length
        {
            get
            {
                return (float)Math.Sqrt((double)this.LengthSq);
            }
        }

        public static Vector2D Zero
        {
            get
            {
                return default(Vector2D);
            }
        }

        public void Normalize()
        {
            if (!this.IsZero)
            {
                float length = this.Length;
                this /= length;
            }
        }

        public override string ToString()
        {
            return string.Concat(new string[]
            {
                "[",
                this.m_x.ToString("f3"),
                ",",
                this.m_y.ToString("f3"),
                "]"
            });
        }

        public int Sign(Vector2D v)
        {
            if (this.m_y * v.x > this.m_x * v.y)
            {
                return -1;
            }
            return 1;
        }

        public Vector2D Perp()
        {
            return new Vector2D(-this.y, this.x);
        }

        public void Truncate(float max)
        {
            if (!this.IsZero && this.LengthSq > max * max)
            {
                this.Normalize();
                this *= max;
            }
        }

        public static float Distance(Vector2D v0, Vector2D v1)
        {
            return (float)Math.Sqrt((double)Vector2D.DistanceSq(v0, v1));
        }

        public static float DistanceSq(Vector2D v0, Vector2D v1)
        {
            return (v0 - v1).LengthSq;
        }

        public void Reflect(Vector2D norm)
        {
            norm.Normalize();
            this += this * norm * 2f * -norm;
        }

        public static Vector2D Vec2DNormalize(Vector2D v)
        {
            float lengthSq = v.LengthSq;
            if (lengthSq > 1.401298E-45f)
            {
                v /= lengthSq;
                return v;
            }
            return default(Vector2D);
        }

        public bool isSecondInFOVOfFirst(Vector2D posFirst, Vector2D facingFirst, Vector2D posSecond, float fov)
        {
            Vector2D v = Vector2D.Vec2DNormalize(posSecond - posFirst);
            return facingFirst * v >= (float)Math.Cos((double)(fov / 2f));
        }

        public void SetZero()
        {
            this.m_x = 0f;
            this.m_y = 0f;
        }

        public static Vector2D operator -(Vector2D v)
        {
            return new Vector2D(-v.m_x, -v.m_y);
        }

        public static Vector2D operator +(Vector2D v0, Vector2D v1)
        {
            return new Vector2D
            {
                m_x = v0.m_x + v1.m_x,
                m_y = v0.m_y + v1.m_y
            };
        }

        public static Vector2D operator -(Vector2D v0, Vector2D v1)
        {
            return new Vector2D
            {
                m_x = v0.m_x - v1.m_x,
                m_y = v0.m_y - v1.m_y
            };
        }

        public static bool operator ==(Vector2D v0, Vector2D v1)
        {
            return v0.m_x == v1.m_x && v0.m_y == v1.m_y;
        }

        public static bool operator !=(Vector2D v0, Vector2D v1)
        {
            return v0.m_x != v1.m_x || v0.m_y != v1.m_y;
        }

        public static float operator *(Vector2D v0, Vector2D v1)
        {
            return v0.m_x * v1.m_x + v0.m_y * v1.m_y;
        }

        public static Vector2D operator *(Vector2D v0, float mul)
        {
            Vector2D result;
            result.m_x = v0.m_x * mul;
            result.m_y = v0.m_y * mul;
            return result;
        }

        public static Vector2D operator *(float mul, Vector2D v0)
        {
            Vector2D result;
            result.m_x = v0.m_x * mul;
            result.m_y = v0.m_y * mul;
            return result;
        }

        public static Vector2D operator /(Vector2D v0, float div)
        {
            if (div == 0f || float.IsNaN(div))
            {
                return default(Vector2D);
            }
            return new Vector2D
            {
                m_x = v0.m_x / div,
                m_y = v0.m_y / div
            };
        }

        private float m_x;

        private float m_y;
    }
}
