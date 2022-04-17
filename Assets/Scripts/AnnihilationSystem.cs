using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vega
{
    public class AnnihilationSystem : MonoBehaviour
    {
        public static AnnihilationSystem instance;

        public ParticleSystem explosionFlash;

        List<StarController> matterStars = new List<StarController>();
        public List<StarController> antimatterStars = new List<StarController>();

        Vector2[] antimatterPoints;
        public List<PlayerController> players = new List<PlayerController>();
        public AiController aiController;

        void Awake()
        {
            instance = this;
            antimatterPoints = new Vector2[0];
        }

        void Update()
        {
            int nAntimatter = antimatterStars.Count;

            if (nAntimatter == 0)
            {
                return;
            }

            for (int i = 0; i < nAntimatter; i++)
            {
                antimatterPoints[i] = antimatterStars[i].transform.position;
            }

            KdTree tree = KdTree.MakeFromPoints(antimatterPoints);
            CheckForPlayerDefeat(tree);

            int nMatter = matterStars.Count;

            if (nMatter == 0)
            {
                return;
            }

            for (int i = 0; i < nMatter; i++)
            {
                Vector2 pos = matterStars[i].transform.position;
                int neighbour = tree.FindNearest(pos);

                if (
                    neighbour != -1 &&
                    (antimatterPoints[neighbour] - pos).sqrMagnitude < 0.02f
                )
                {
                    matterStars[i].markAnnihilated = true;
                    antimatterStars[neighbour].markAnnihilated = true;

                    EmitFlash((antimatterPoints[neighbour] + pos) / 2f);
                    Scores.instance.AddToScore();
                }
            }


            StarSystem.instance.RemoveAnnihilated();
        }

        void CheckForPlayerDefeat(KdTree tree)
        {
            for (int i = 0; i < players.Count; i++)
            {
                PlayerController pl = players[i];
                Vector2 pos = pl.transform.position;
                int neighbour = tree.FindNearest(pos);

                if (
                    neighbour != -1 &&
                    (antimatterPoints[neighbour] - pos).sqrMagnitude < 0.02f
                )
                {
                    players.Remove(pl);
                    Destroy(pl.gameObject);
                    CheckForGameOver();
                }
            }
        }

        void CheckForGameOver()
        {
            if (aiController != null)
            {
                if (players.Count == 0)
                {
                    SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
                }
            }
            else
            {
                int nMatter = 0;
                int nAntimatter = 0;

                for(int i = 0; i < players.Count; i++)
                {
                    if(players[i].isAntimatter)
                    {
                        nAntimatter++;
                    }
                    else
                    {
                        nMatter++;
                    }
                }

                if(nMatter == 0 || nMatter == 0)
                {
                    SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
                }
            }
        }

        void EmitFlash(Vector2 pos)
        {
            ParticleSystem.EmitParams emitParams;
            emitParams = new ParticleSystem.EmitParams();

            emitParams.position = new Vector3(pos.x, pos.y, 5f);
            emitParams.applyShapeToPosition = true;
            emitParams.velocity = Vector3.zero;

            explosionFlash.Emit(emitParams, 1);
        }

        public void Add(StarController star)
        {
            if (star.isAntimatter)
            {
                antimatterStars.Add(star);
            }
            else
            {
                matterStars.Add(star);
            }

            antimatterPoints = new Vector2[antimatterStars.Count];
        }

        public void Remove(StarController star)
        {
            if (star.isAntimatter)
            {
                antimatterStars.Remove(star);
            }
            else
            {
                matterStars.Remove(star);
            }

            antimatterPoints = new Vector2[antimatterStars.Count];
        }
    }
}
