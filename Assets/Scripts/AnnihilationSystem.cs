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
        Vector2[] matterPoints;
        public List<PlayerController> players = new List<PlayerController>();
        public AiController aiController;

        void Awake()
        {
            instance = this;
            antimatterPoints = new Vector2[0];
            matterPoints = new Vector2[0];
        }

        void Update()
        {
            int nAntimatter = antimatterStars.Count;
            int nMatter = matterStars.Count;

            for (int i = 0; i < nAntimatter; i++)
            {
                antimatterPoints[i] = antimatterStars[i].transform.position;
            }

            for (int i = 0; i < nMatter; i++)
            {
                matterPoints[i] = matterStars[i].transform.position;
            }

            KdTree antiMatterTree = KdTree.MakeFromPoints(antimatterPoints);
            KdTree matterTree = KdTree.MakeFromPoints(matterPoints);

            CheckForMatterPlayerDefeat(antiMatterTree, matterTree);

            if (nAntimatter == 0 || nMatter == 0)
            {
                return;
            }

            for (int i = 0; i < nMatter; i++)
            {
                Vector2 pos = matterStars[i].transform.position;
                int neighbour = antiMatterTree.FindNearest(pos);

                if (
                    neighbour != -1 &&
                    (antimatterPoints[neighbour] - pos).sqrMagnitude < 0.02f
                )
                {
                    matterStars[i].markAnnihilated = true;
                    antimatterStars[neighbour].markAnnihilated = true;

                    EmitFlash((antimatterPoints[neighbour] + pos) / 2f);

                    Scores.instance.AddToScore(matterStars[i].playerId);
                    Scores.instance.AddToScore(antimatterStars[i].playerId);
                }
            }

            StarSystem.instance.RemoveAnnihilated();
        }

        void CheckForMatterPlayerDefeat(KdTree antiMatterTree, KdTree matterTree)
        {
            for (int i = 0; i < players.Count; i++)
            {
                PlayerController pl = players[i];

                if (!pl.isAntimatter)
                {
                    if (antiMatterTree != null)
                    {
                        Vector2 pos = pl.transform.position;
                        int neighbour = antiMatterTree.FindNearest(pos);

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
                else
                {
                    if (matterTree != null)
                    {
                        Vector2 pos = pl.transform.position;
                        int neighbour = matterTree.FindNearest(pos);

                        if (
                            neighbour != -1 &&
                            (matterPoints[neighbour] - pos).sqrMagnitude < 0.02f
                        )
                        {
                            players.Remove(pl);
                            Destroy(pl.gameObject);
                            CheckForGameOver();
                        }
                    }
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

                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].isAntimatter)
                    {
                        nAntimatter++;
                    }
                    else
                    {
                        nMatter++;
                    }
                }

                if (nMatter == 0 || nMatter == 0)
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
            matterPoints = new Vector2[matterStars.Count];
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
            matterPoints = new Vector2[matterStars.Count];
        }
    }
}
