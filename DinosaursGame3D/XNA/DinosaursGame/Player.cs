﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DinosaursGame 
{
    class Player : DrawableGameComponent
    {
        // Set the 3D model to draw.
        private Model myModel;
        private ModelBone headBone;

        // The aspect ratio determines how to scale 3d to 2d projection.
        private float aspectRatio;

        private BasicEffect effect;

        // Set the position of the model in world space, and set the rotation.
        public Vector3 position = Vector3.Zero;
        public Vector3 prevPosition = Vector3.Zero;
        private Vector3 destination = Vector3.Zero;
        public float rotation = 3.14f; //-0.5f;
        private float maxSpeed = 5.0f;
        private Vector3 currentVelocity = Vector3.Zero;

        // Set the position of the camera in world space, for our view matrix.
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 2000.0f);
        
        public Player(Game game)
            : base(game)
        {
        }

        public Player(Game game, Vector3 position)
            : base(game)
        {
            this.position = position;
            prevPosition = position;
        }

        public override void Initialize()
        {
            effect = new BasicEffect(
               Game.GraphicsDevice);
            effect.AmbientLightColor = Vector3.One;
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.DiffuseColor =
                                        Vector3.One;
            effect.DirectionalLight0.Direction =
                     Vector3.Normalize(Vector3.One);
            effect.LightingEnabled = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            myModel = Game.Content.Load<Model>("Player/Apatosaurus06_rig");

            headBone = myModel.Bones["Head"];
        
            aspectRatio = (float) Game.GraphicsDevice.Viewport.Width /
                (float)Game.GraphicsDevice.Viewport.Height;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TurnTowardDestination();
            MoveTowardDestination();
        }

        private void TurnTowardDestination()
        {
            if (destination.X - position.X != 0)
            {
                float angle = (float)Math.Atan((destination.Z - position.Z) / (destination.X - position.X));
                //float angle = (float)Math.Atan((position.Z - prevPosition.Z) / (position.X - prevPosition.X));
                if (position.X < prevPosition.X)
                {
                    angle += (float)Math.PI;
                }
                
                rotation = angle;
            }
        }

        private void MoveTowardDestination()
        {
            //currentVelocity.X = (float)(maxSpeed * Math.Cos(angle));
            //currentVelocity.Y = (float)(maxSpeed * Math.Sin(angle));
            //currentVelocity = Util.Vec3Diff(position, destination);

            // Make sure the player is not traveling too fast
            //currentVelocity.X = (currentVelocity.X > maxSpeed) ? maxSpeed : currentVelocity.X;
            //currentVelocity.Y = (currentVelocity.Y > maxSpeed) ? maxSpeed : currentVelocity.Y;
            //currentVelocity.Z = (currentVelocity.Z > maxSpeed) ? maxSpeed : currentVelocity.Z;

            //position = Util.Vec3Sum(position, currentVelocity); // destination;
            prevPosition = position;
            position = destination;
        }

        public override void Draw(GameTime gameTime)
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                // This is where the mesh orientation is set, as well as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(rotation)
                        * Matrix.CreateTranslation(position);
                    effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                        aspectRatio, 1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }

            // Move just the head
            /*Vector3 oringalTrans = headBone.Transform.Translation;
            headBone.Transform *= Matrix.CreateTranslation(new Vector3(1000, 1000, 1000));
            Vector3 newTrans = headBone.Transform.Translation;
            headBone.Transform *= Matrix.CreateTranslation(oringalTrans - newTrans);
            */
            base.Draw(gameTime);
        }

        public void goTo(Vector3 destination)
        {
            this.destination = destination;
        }


    }
}
