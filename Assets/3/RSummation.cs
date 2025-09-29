using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;


/*
Using Unity’s C# Job System, calculate the sum of the R channel, for each texture 
element of a texture. To do this, split the texture into four regions of equal size, the 
operation should be processed by jobs running in parallel.
*/

namespace QuestionThree {

    public struct SumRValuesJobParallelFor : IJobParallelFor
    {
        [ReadOnly] public NativeArray<int> index;
        [ReadOnly] public NativeArray<float> array;
        [ReadOnly] public NativeArray<float> arrayB;
        [ReadOnly] public NativeArray<float> arrayC;
        [ReadOnly] public NativeArray<float> arrayD;

        [WriteOnly] public NativeArray<float> result;

        public void Execute(int i)
        {
            var my_array = array;

            switch (index[i])
            {
                case 0: my_array = array; break;
                case 1: my_array = arrayB; break;
                case 2: my_array = array; break;
                case 3: my_array = array; break;
            }

            var sum = 0f;
            for (int j = 0; j < array.Length; j++)
                sum += array[j];
            result[i] = sum;
        }
    }


    public class RSummation : MonoBehaviour
    {
        private NativeArray<float> a;
        private NativeArray<float> b;
        private NativeArray<float> c;
        private NativeArray<float> d;

        private NativeArray<float> result;
        private NativeArray<int> assignedIndices;

        SumRValuesJobParallelFor myParallelJob;
        JobHandle myParallelJobHandler;

        [SerializeField] private Texture2D sampleTexture2D;

        private void InitializeAll(int regionLength, int regionCount)
        {
            a = new NativeArray<float>(regionLength, Allocator.TempJob);
            b = new NativeArray<float>(regionLength, Allocator.TempJob);
            c = new NativeArray<float>(regionLength, Allocator.TempJob);
            d = new NativeArray<float>(regionLength, Allocator.TempJob);
            result = new NativeArray<float>(regionCount, Allocator.TempJob);
            assignedIndices = new NativeArray<int>(regionCount, Allocator.TempJob);
        }

        private void DisposeAll()
        {
            a.Dispose();
            b.Dispose();
            c.Dispose();
            d.Dispose();
            assignedIndices.Dispose();
            result.Dispose();
        }

        void AssignRValues(Color[] pixels)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                int result = i % 4;
                int index = i / 4;
                switch (result)
                {
                    case 0: a[index] = pixels[i].r; break;
                    case 1: b[index] = pixels[i].r; break;
                    case 2: c[index] = pixels[i].r; break;
                    case 3: d[index] = pixels[i].r; break;
                }
            }
        }

        void DispatchMyJob()
        {
            Color[] pixels = sampleTexture2D.GetPixels();
            int regionlength = Mathf.FloorToInt(pixels.Length / 4);

            InitializeAll(regionlength, 4);
            AssignRValues(pixels);

            assignedIndices[0] = 0;
            assignedIndices[1] = 1;
            assignedIndices[2] = 2;
            assignedIndices[3] = 3;

            myParallelJob = new SumRValuesJobParallelFor() { index = assignedIndices, array = a, arrayB = b, arrayC = c, arrayD = d, result = result };
            myParallelJobHandler = myParallelJob.ScheduleByRef(4, 1, myParallelJobHandler); // 32/64 for jobs with little work, 1 otherwise 'https://docs.unity3d.com/ScriptReference/Unity.Jobs.IJobParallelFor.html'
        }

        float GetFinalSum()
        {
            float sum = 0f;
            for (int i = 0; i < result.Length; i++)
                sum += myParallelJob.result[i];
            return sum;
        }

        void CompleteMyJob()
        {
            myParallelJobHandler.Complete();
            Debug.Log(GetFinalSum());
            DisposeAll();
        }

        private void Awake()
        {
            if (sampleTexture2D == null) return;
            DispatchMyJob();
        }

        void Start()
        {
            if (sampleTexture2D == null) return;
            CompleteMyJob();
        }
    }
}