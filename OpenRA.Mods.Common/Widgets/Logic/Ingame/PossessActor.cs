using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using OpenRA.Network;
using OpenRA.Widgets;
using OpenRA.Mods.Common.Orders;
using OpenRA.Mods.Common.Traits;
using System.Linq;
using OpenRA.Traits;
using OpenRA.Mods.Common.Widgets.Logic.Ingame;
using OpenRA.Mods.Common.Widgets;
using g_Cmd = OpenRA.Mods.Common.Widgets.CommandBarLogic;
using System.Runtime.Intrinsics.X86;

namespace OpenRA.Mods.Common.Widgets // Replace "YourNamespace" with the appropriate namespace for your project
{
    public class PossessActor : Widget
    {
        private readonly World world;
        private CommandBarLogic Cmd;
        public Actor PossessedActor = null;
        public KeyInput e;

        public bool bMoveUp = false;
        public bool bMoveDown = false;
        public bool bMoveLeft = false;
        public bool bMoveRight = false;
        
        CPos TargetDestination = new CPos(-1, -1);

        [ObjectCreator.UseCtor]
        public PossessActor(World world, CommandBarLogic CommandBarLogicRef1)
        {
            this.world = world;
            Cmd = CommandBarLogicRef1;
            
        }

        public void PossessSelectedActor(Actor A) {
            int check = 0;
            PossessedActor = A;
            world.InputsConsumable.Clear();
            world.bPossessedActor = true;
            TextNotificationsManager.AddTransientLine("Actor possessed", world.LocalPlayer);     

        }

        public void LockCameraOnActor() {


        }

        public void UnpossessActor() {
            PossessedActor = null;
            world.InputsConsumable.Clear();
            world.bPossessedActor = false;
            TextNotificationsManager.AddTransientLine("Actor Unpossessed", world.LocalPlayer);     

        }

        public void MoveUp() {
            // if(e.Key == Keycode.W && e.Event == KeyInputEvent.Down)
			// if(e.IsRepeat) {
			// 	return true;
			// 	if(e.Key == Keycode.W || e.Key == Keycode.A || e.Key == Keycode.S || e.Key == Keycode.D)
			// 	OnKeyPress(e);	
            bMoveUp = true;
            if(bMoveRight) {
                MoveTopRight();
                return;
                } else if (bMoveLeft){
                MoveTopLeft();
                return;
            }

                TextNotificationsManager.AddTransientLine("Move up activated", world.LocalPlayer);     
                 CPos AdvanceDestination = new CPos(PossessedActor.Location.X+0, PossessedActor.Location.Y-1);
                 if(TargetDestination == AdvanceDestination) 
                 return;
                 else {
                    bMoveUp = true;
                    TargetDestination = AdvanceDestination;
                    MoveTo(TargetDestination);
                 }               
        }

        public void MoveDown() {
            bMoveDown = true;
            if(bMoveLeft) {
                MoveBottomLeft();
                return;
                } else if (bMoveRight){
                MoveBottomRight();
                return;
            }
                TextNotificationsManager.AddTransientLine("Move Down activated", world.LocalPlayer);     
                 CPos AdvanceDestination = new CPos(PossessedActor.Location.X+0, PossessedActor.Location.Y+1);
                 if(TargetDestination == AdvanceDestination) 
                 return;
                 else {
                    bMoveDown = true;
                    TargetDestination = AdvanceDestination;
                    MoveTo(TargetDestination);
                 }
        }

            public void MoveLeft() {
                bMoveLeft = true;
                if(bMoveUp) {
                MoveTopLeft();
                return;
                } else if (bMoveDown){
                MoveBottomLeft();
                return;
            }
                TextNotificationsManager.AddTransientLine("Move Left activated", world.LocalPlayer);     
                 CPos AdvanceDestination = new CPos(PossessedActor.Location.X-1, PossessedActor.Location.Y);
                 if(TargetDestination == AdvanceDestination) 
                 return;
                 else {
                    // bMoveLeft = true;
                    TargetDestination = AdvanceDestination;
                    MoveTo(TargetDestination);
                 }
        }

                public void MoveRight() {
                    bMoveRight = true;
                if(bMoveUp) {
                MoveTopRight();
                return;
                } else if(bMoveDown) {
                MoveBottomRight();
                return;
                }


                TextNotificationsManager.AddTransientLine("Move Right", world.LocalPlayer);     
                 CPos AdvanceDestination = new CPos(PossessedActor.Location.X+1, PossessedActor.Location.Y);
                 if(TargetDestination == AdvanceDestination) 
                 return;
                 else {
                    bMoveRight = true;
                    TargetDestination = AdvanceDestination;
                    MoveTo(TargetDestination);
                 }
        }

        
                public void MoveTopRight() {
                TextNotificationsManager.AddTransientLine("Move TopRight", world.LocalPlayer);     
                 CPos AdvanceDestination = new CPos(PossessedActor.Location.X+2, PossessedActor.Location.Y-2);
                 if(TargetDestination == AdvanceDestination) 
                 return;
                 else {
                    TargetDestination = AdvanceDestination;
                    MoveTo(TargetDestination);
                 }
                }

            public void MoveBottomRight() {
                TextNotificationsManager.AddTransientLine("Move BottomRight", world.LocalPlayer);     
                 CPos AdvanceDestination = new CPos(PossessedActor.Location.X+2, PossessedActor.Location.Y+2);
                 if(TargetDestination == AdvanceDestination) 
                 return;
                 else {
                    TargetDestination = AdvanceDestination;
                    MoveTo(TargetDestination);
                 }
                }

            public void MoveBottomLeft() {
                TextNotificationsManager.AddTransientLine("MoveBottomLeft activated", world.LocalPlayer);     
                 CPos AdvanceDestination = new CPos(PossessedActor.Location.X-2, PossessedActor.Location.Y+2);
                 if(TargetDestination == AdvanceDestination) 
                 return;
                 else {
                    TargetDestination = AdvanceDestination;
                    MoveTo(TargetDestination);
                 }
                }

            public void MoveTopLeft() {
                TextNotificationsManager.AddTransientLine("MoveTopLeft activated", world.LocalPlayer);     
                 CPos AdvanceDestination = new CPos(PossessedActor.Location.X-2, PossessedActor.Location.Y-2);
                 if(TargetDestination == AdvanceDestination) 
                 return;
                 else {
                    TargetDestination = AdvanceDestination;
                    MoveTo(TargetDestination);
                 }
            }

        public void MoveTo(CPos Dest) {
            string OrderType = "Move";
            Actor[] selectedActorsArray = new Actor[] { PossessedActor };
            var currentPlayer = world.LocalPlayer.PlayerActor;
            Order o = new Order(OrderType, currentPlayer, Target.FromCell(world, Dest), false, null, selectedActorsArray);
            o.ExtraData = currentPlayer.ActorID;
            world.IssueOrder(o);
        }

        public void StopMovement(Keycode Key) {
            //may need to check if its not moving already in any other direction.
            TextNotificationsManager.AddTransientLine("Stopped Key: " + Key, world.LocalPlayer);     

            switch(Key) {
                case Keycode.W:
                bMoveUp = false;
                break;
                case Keycode.A:
                bMoveLeft = false;
                break;
                case Keycode.S:
                bMoveDown = false;
                break;
                case Keycode.D:
                bMoveRight = false;
                break;
            }
        }

        public void HandleInput() {
            if(PossessedActor== null)
            return;
            for(int i = 0; i < world.InputsConsumable.Count; ++i) {
                KeyInput e = world.InputsConsumable[i];
                if(e.Key == Keycode.W && e.Event == KeyInputEvent.Down) {
                TextNotificationsManager.AddTransientLine("Key W", world.LocalPlayer);     
                bMoveUp = true;
                }
                else if(e.Key == Keycode.S && e.Event == KeyInputEvent.Down){
                TextNotificationsManager.AddTransientLine("Key S", world.LocalPlayer);     
                bMoveDown = true;
                }
                else if(e.Key == Keycode.A && e.Event == KeyInputEvent.Down){
                TextNotificationsManager.AddTransientLine("Key A", world.LocalPlayer);     
                bMoveLeft = true;
                }
                else if(e.Key == Keycode.D && e.Event == KeyInputEvent.Down){
                TextNotificationsManager.AddTransientLine("Key D", world.LocalPlayer);     
                bMoveRight = true;
                }
                else if(e.Event == KeyInputEvent.Up) {
                    StopMovement(e.Key); //might not be necessary.
                }
            }
            if(bMoveUp)
                MoveUp();
            if(bMoveDown)
                MoveDown();
            if(bMoveLeft)
                MoveLeft();
            if(bMoveRight)
                MoveRight();

            world.InputsConsumable.Clear();
        }

        public override bool HandleKeyPress(KeyInput e)
		{

				int check = 0;
                check = 30;
                return true;

		}



    }

}

