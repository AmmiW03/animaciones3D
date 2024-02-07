using System;
using System.Diagnostics;
using System.Windows.Forms;
using OpenTK;
using NAudio;
using OpenTK.Graphics.OpenGL;
using NAudio.Wave;

namespace animaciones3D
{
    public partial class Form1 : Form
    {
        private GLControl glControl = new GLControl();

        public Form1()
        {
            InitializeComponent();

            // Agregar el control GLControl al formulario
            Controls.Add(glControl);

            glControl.Load += Form1_Load;
            glControl.Paint += glControl_Paint;
            glControl.Height = 500;
            glControl.Width = 500;

            // Llama al método para reproducir el audio automáticamente al cargar el formulario
            ReproducirAudio();
        }

        private void ReproducirAudio()
        {
            try
            {
                // Ruta del archivo de audio
                string rutaArchivo = "Recursos/archivo.wav";

                // Crear un lector de audio con NAudio
                using (var reader = new WaveFileReader(rutaArchivo))
                {
                    // Crear un reproductor de audio
                    using (var waveOut = new WaveOutEvent())
                    {
                        waveOut.Init(reader);

                        // Reproducir el audio
                        waveOut.Play();

                        // Puedes agregar código adicional aquí si deseas realizar acciones mientras se reproduce el audio
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al reproducir el audio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            glControl.MakeCurrent(); // Establecer el contexto OpenGL actual

            // Configuración básica de OpenGL
            GL.ClearColor(System.Drawing.Color.Black);
            GL.Enable(EnableCap.DepthTest);
        }

        private float angle = 0.0f; // Variable para el ángulo de rotación
        private float rotationSpeed = 0.01f; // Velocidad de rotación
        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Configurar la matriz de proyección
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)glControl.Width / glControl.Height, 0.1f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            // Configurar la matriz del modelo-vista (cámara)
            Matrix4 modelview = Matrix4.LookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);

            // Aplicar rotación al modelo-vista
            GL.LoadMatrix(ref modelview);
            GL.Rotate(angle, 0.0f, 1.0f, 0.0f); // Rotar alrededor del eje Y

            // Renderizar la pirámide en una posición
            GL.PushMatrix(); // Guardar la matriz actual
            GL.Translate(-2.0f, 0.0f, 0.0f); // Trasladar la pirámide en el eje X
            RenderizarPiramide();
            GL.PopMatrix(); // Restaurar la matriz original

            // Renderizar el cubo en otra posición
            GL.PushMatrix(); // Guardar la matriz actual
            GL.Translate(2.0f, 0.0f, 0.0f); // Trasladar el cubo en el eje X
            RenderizarCubo();
            GL.PopMatrix(); // Restaurar la matriz original

            glControl.SwapBuffers();

            // Incrementar el ángulo para la siguiente frame con velocidad de rotación más lenta
            angle += rotationSpeed;
            if (angle >= 360.0f)
            {
                angle -= 360.0f;
            }

            //Solicitar un nuevo renderizado
            glControl.Invalidate();
        }

        private void RenderizarPiramide()
        {
            // Coordenadas de la base de la pirámide
            float baseSize = 1.5f;
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(1.0f, 1.0f, 0.0f); // Amarillo

            GL.Vertex3(-baseSize, -1.0f, -baseSize);
            GL.Vertex3(baseSize, -1.0f, -baseSize);
            GL.Vertex3(baseSize, -1.0f, baseSize);
            GL.Vertex3(-baseSize, -1.0f, baseSize);

            GL.End();

            // Coordenadas del triángulo que forma la pirámide
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(1.0f, 0.0f, 0.0f); // Rojo

            GL.Vertex3(-baseSize, -1.0f, -baseSize);
            GL.Vertex3(baseSize, -1.0f, -baseSize);
            GL.Vertex3(0.0f, 1.0f, 0.0f);

            GL.Color3(0.0f, 1.0f, 0.0f); // Verde

            GL.Vertex3(baseSize, -1.0f, -baseSize);
            GL.Vertex3(baseSize, -1.0f, baseSize);
            GL.Vertex3(0.0f, 1.0f, 0.0f);

            GL.Color3(0.0f, 0.0f, 1.0f); // Azul

            GL.Vertex3(baseSize, -1.0f, baseSize);
            GL.Vertex3(-baseSize, -1.0f, baseSize);
            GL.Vertex3(0.0f, 1.0f, 0.0f);

            GL.Color3(1.0f, 0.0f, 1.0f); // Morado

            GL.Vertex3(-baseSize, -1.0f, baseSize);
            GL.Vertex3(-baseSize, -1.0f, -baseSize);
            GL.Vertex3(0.0f, 1.0f, 0.0f);

            GL.End();
        }

        private void RenderizarCubo()
        {
            float halfSize = 0.5f; // La mitad del tamaño del cubo

            GL.Begin(PrimitiveType.Quads);

            // Cara frontal
            GL.Color3(1.0f, 0.0f, 0.0f); // Rojo
            GL.Vertex3(-halfSize, -halfSize, halfSize);
            GL.Vertex3(halfSize, -halfSize, halfSize);
            GL.Vertex3(halfSize, halfSize, halfSize);
            GL.Vertex3(-halfSize, halfSize, halfSize);

            // Cara trasera
            GL.Color3(0.0f, 1.0f, 0.0f); // Verde
            GL.Vertex3(halfSize, -halfSize, -halfSize);
            GL.Vertex3(-halfSize, -halfSize, -halfSize);
            GL.Vertex3(-halfSize, halfSize, -halfSize);
            GL.Vertex3(halfSize, halfSize, -halfSize);

            // Cara izquierda
            GL.Color3(0.0f, 0.0f, 1.0f); // Azul
            GL.Vertex3(-halfSize, -halfSize, -halfSize);
            GL.Vertex3(-halfSize, -halfSize, halfSize);
            GL.Vertex3(-halfSize, halfSize, halfSize);
            GL.Vertex3(-halfSize, halfSize, -halfSize);

            // Cara derecha
            GL.Color3(1.0f, 1.0f, 0.0f); // Amarillo
            GL.Vertex3(halfSize, -halfSize, halfSize);
            GL.Vertex3(halfSize, -halfSize, -halfSize);
            GL.Vertex3(halfSize, halfSize, -halfSize);
            GL.Vertex3(halfSize, halfSize, halfSize);

            // Cara superior
            GL.Color3(1.0f, 0.0f, 1.0f); // Morado
            GL.Vertex3(-halfSize, halfSize, halfSize);
            GL.Vertex3(halfSize, halfSize, halfSize);
            GL.Vertex3(halfSize, halfSize, -halfSize);
            GL.Vertex3(-halfSize, halfSize, -halfSize);

            // Cara inferior
            GL.Color3(0.0f, 1.0f, 1.0f); // Cian
            GL.Vertex3(-halfSize, -halfSize, -halfSize);
            GL.Vertex3(halfSize, -halfSize, -halfSize);
            GL.Vertex3(halfSize, -halfSize, halfSize);
            GL.Vertex3(-halfSize, -halfSize, halfSize);

            GL.End();
        }


    }
}
