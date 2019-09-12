using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace Unity.GPUAnimation
{
    public class InstancedSkinningDrawer : IDisposable
    {
        private const int PreallocatedBufferSize = 1024;

        private ComputeBuffer argsBuffer;

        private readonly uint[] indirectArgs = new uint[5] { 0, 0, 0, 0, 0 };

        private ComputeBuffer textureCoordinatesBuffer;
        private ComputeBuffer objectToWorldBuffer;

        private Material material;

        private Mesh mesh;
        
        public unsafe InstancedSkinningDrawer(Material srcMaterial, Mesh meshToDraw, AnimationTextures animTexture)
        {
            this.mesh = meshToDraw;
            this.material = new Material(srcMaterial);

			// 需要的ComputeBuffer只有76字节，这也是CPU占用低的主要原因，传递的数据是顶点的转移矩阵和它所在的材质中的坐标
            argsBuffer = new ComputeBuffer(1, indirectArgs.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            indirectArgs[0] = mesh.GetIndexCount(0);
            indirectArgs[1] = (uint)0;
            argsBuffer.SetData(indirectArgs);

            objectToWorldBuffer = new ComputeBuffer(PreallocatedBufferSize, 16 * sizeof(float));
            textureCoordinatesBuffer = new ComputeBuffer(PreallocatedBufferSize, 3 * sizeof(float));
	
            this.material.SetBuffer("textureCoordinatesBuffer", textureCoordinatesBuffer);
            this.material.SetBuffer("objectToWorldBuffer", objectToWorldBuffer);
            this.material.SetTexture("_AnimationTexture0", animTexture.Animation0);
            this.material.SetTexture("_AnimationTexture1", animTexture.Animation1);
            this.material.SetTexture("_AnimationTexture2", animTexture.Animation2);
        }

        public void Dispose()
        {
            UnityEngine.Object.DestroyImmediate(material);
		
            if (argsBuffer != null) argsBuffer.Dispose();
            if (objectToWorldBuffer != null) objectToWorldBuffer.Dispose();
            if (textureCoordinatesBuffer != null) textureCoordinatesBuffer.Dispose();
        }
        
        public void Draw(NativeArray<float3> TextureCoordinates, NativeArray<float4x4> ObjectToWorld, ShadowCastingMode shadowCastingMode, bool receiveShadows)
        {
            // CHECK: Systems seem to be called when exiting playmode once things start getting destroyed, such as the mesh here.
            if (mesh == null || material == null) 
                return;

            int count = TextureCoordinates.Length;
            if (count == 0) 
                return;

            if (count > objectToWorldBuffer.count)
            {
                objectToWorldBuffer.Dispose();
                textureCoordinatesBuffer.Dispose();
                
                objectToWorldBuffer = new ComputeBuffer(TextureCoordinates.Length, 16 * sizeof(float));
                textureCoordinatesBuffer = new ComputeBuffer(TextureCoordinates.Length, 3 * sizeof(float));
            }

            this.material.SetBuffer("textureCoordinatesBuffer", textureCoordinatesBuffer);
            this.material.SetBuffer("objectToWorldBuffer", objectToWorldBuffer);
            
            Profiler.BeginSample("Modify compute buffers");

            Profiler.BeginSample("Shader set data");

            objectToWorldBuffer.SetData(ObjectToWorld, 0, 0, count);
            textureCoordinatesBuffer.SetData(TextureCoordinates, 0, 0, count);
            
            Profiler.EndSample();

            Profiler.EndSample();

            //indirectArgs[1] = (uint)data.Count;
            indirectArgs[1] = (uint)count;
            argsBuffer.SetData(indirectArgs);

			// 实现在场景中绘制制定数量的角色
			DrawMeshInstancedIndirect(mesh, 0, material, new Bounds(Vector3.zero, 1000000 * Vector3.one), argsBuffer, 0, new MaterialPropertyBlock(), shadowCastingMode, receiveShadows);
        }

		/// <summary>
		/// Draw the same mesh multipe times using GPU instancing
		/// similar to Graphics.DrawMeshInstanced, this function draws manay instances of the same meshm but unlike that method, the arguments for how manay instances to draw come from bufferWithArgs
		/// Use this function in situations where you want to draw the same mesh for a particular amount of times using an instanced shader. Meshes
		/// are not further culled by the view frustum or baked occluders, nor sorted for transparency or z efficiency.
		/// Buffer with arguments bufferWithArgs, has five interger numbers at given argsOffset offset : index count per instance, instance count, start index location, base vertex location, start instance location
		/// TODO Script Example
		/// </summary>
		/// <param name="mesh">The mesh to draw</param>
		/// <param name="submeshIndex">Which subset of the mesh to draw. This applies only to meshes that are composed of serveral material.</param>
		/// <param name="material">Material to use</param>
		/// <param name="bounds">The bounding volume surrounding the instances you intend to draw</param>
		/// <param name="bufferWithArgs">The GPU buffer containing the argumenets for how many instances of this mesh to draw</param>
		/// <param name="argsOffset">The byte offset into buffer, where the draw arguments start</param>
		/// <param name="properties">Additional material properties to apply, see MaterialPropertyBlock</param>
		/// <param name="castShadows">Determines whether the mesh can cast shadows</param>
		/// <param name="receiveShadows"><Determines whether the mesh can recieve shadows/param>
		void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
		{
			Graphics.DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows);
		}
	}
}