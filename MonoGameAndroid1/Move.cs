using System.Collections.Generic;
using System.Linq;

namespace MonoGameAndroid1
{
    public class Move
    {
        private Vector2i startPos;
        private Vector2i endPos;
        private List<Vector2i> inBetweenSteps = new List<Vector2i>();
        private List<Vector2i> posToRemovePieces = new List<Vector2i>();

        private Vector2i LastAddedPos => inBetweenSteps.Count > 0 ? inBetweenSteps.Last() : startPos;

        public Move(Vector2i startPos)
        {
            this.startPos = startPos;
        }

        public void AddNewStep(Vector2i nextPos)
        {
            var posToRemove = (nextPos + LastAddedPos) / 2; 
            inBetweenSteps.Add(nextPos);
            posToRemovePieces.Add(posToRemove);
        }
    }
}