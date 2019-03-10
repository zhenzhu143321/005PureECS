using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class MovementSystem : ComponentSystem
{
    //这里声明一个结构, 其中包含我们定义的过滤条件, 也就是必须拥有Position组件才会被注入
    public struct Group
    {
        public readonly int Length;
        public ComponentDataArray<VelocityComponent> Velocities;
        public ComponentDataArray<Position> Positions;
        public ComponentDataArray<PlayerComponent> Players;
    }
    public struct GameObjectGroup
    {
        public readonly int Length;
        public ComponentArray<Transform> Transforms; //该数组可以获取传统的Component
        public ComponentDataArray<VelocityComponent> Velocities;//该数组获取继承IComponentData的
    }
    //然后声明结构类型的字段, 并且加上[Inject]
    [Inject] Group data;
    [Inject] GameObjectGroup go;

    protected override void OnUpdate()
    {
        float deltaTime = Time.deltaTime;
        for (int i = 0; i < data.Length; i++)
        {
            float3 pos = data.Positions[0].Value;             //Read
            float3 vector = data.Velocities[0].moveDir;       //Read
            pos += vector * deltaTime; //Move
            data.Positions[0] = new Position { Value = pos }; //Write
        }

        for (int i = 0; i < go.Length; i++)
        {
            float3 pos = go.Transforms[i].position; //Read
            float3 vector = go.Velocities[i].moveDir; //Read
            pos += vector * deltaTime; //Move
            go.Transforms[i].position = pos; //Write
        }
    }
}
