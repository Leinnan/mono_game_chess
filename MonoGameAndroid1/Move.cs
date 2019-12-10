using System.Collections.Generic;
using System.Linq;

namespace MonoGameAndroid1
{
    public class Move
    {
        private Vector2i startPos;
        private Vector2i endPos;
        private List<Vector2i> multipleSteps = new List<Vector2i>();
        private List<Vector2i> posToRemovePieces = new List<Vector2i>();

        private Vector2i LastAddedPos => multipleSteps.Count > 0 ? multipleSteps.Last() : StartPos;

        public List<Vector2i> PosToRemovePieces => posToRemovePieces;

        public Vector2i StartPos => startPos;

        public Vector2i EndPos => endPos;

        public Move(Vector2i startPos)
        {
            this.startPos = startPos;
        }

        public Move(Vector2i startPos, Vector2i endPos)
        {
            this.startPos = startPos;
            this.endPos = endPos;
        }

        public void AddNewStep(Vector2i nextPos)
        {
            var posToRemove = (nextPos + LastAddedPos) / 2;
            multipleSteps.Add(nextPos);
            posToRemovePieces.Add(posToRemove);
            endPos = nextPos;
        }
    }
}