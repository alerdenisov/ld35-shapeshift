﻿using UnityEngine;
using System.Collections;

using Cubiquity.Impl;

namespace Cubiquity
{
	/// Stores the result of picking a voxel.
	/**
	 * This structure is used to return the result of picking a specific voxel. That is, the voxel space position is the
	 * coordinates of the picked voxel and always consists of integer values. However, the volume could have some arbitrary
	 * transform applied to it and so the world space position consists of floating point values.
	 */
	public struct PickVoxelResult
	{
		public Vector3i volumeSpacePos;
		public Vector3 worldSpacePos;
	}
	
	/// Stores the result of picking a point on a volume surface.
	/**
	 * This structure is used to return the result of picking the surface of the volume, rather than picking the underlying
	 * voxels. The voxels themselves are always on integer positions in volume space, but the generated surface usually
	 * passes *between* voxels. Therefore both the volume space and world space positions are provided as floating point values.
	 */
	public struct PickSurfaceResult
	{
		public Vector3 volumeSpacePos;
		public Vector3 worldSpacePos;
	}
	
	/// Contains methods for picking directly against the volume data (rather than the mesh representation).
	/**
	 * Unity already provides raycasting methods to detect intersections with a mesh or collider, but sometimes it is useful to perform a 
	 * raycast against the actual voxels. For example, this will still work when a collider is not present (such as in edit mode).
	 * 
	 * The functions operate by stepping along the ray in small increments and repeatedly checking whether the current position is a 'solid' voxel.
	 * This means it can be slow if the ray is cast over a long distance but very fast over short distances. The decision of whether to use voxel-based
	 * or collider-based raycasting is highly dependant on the particular problem you are trying to solve.
	 */
	public class Picking
	{
		private static void validateDistance(float distance)
		{
			// If the user attemps to cast a long ray then this can be a
			// slow process, as the ray moves forward one voxel at a time.
			const float distanceLimit = 10000.0f;
			if(distance > distanceLimit)
			{
				Debug.LogWarning("Provided picking distance of " + distance + "is very large and may cause performance problems");
			}
		}
		
		/// Picks a point on the surface of the terrain volume. 
		/**
		 * Note that the result may be slightly inaccurate due to the fixed step size.
		 */
		public static bool PickSurface(TerrainVolume volume, Ray ray, float distance, out PickSurfaceResult pickResult)
		{
			return PickSurface(volume, ray.origin, ray.direction, distance, out pickResult);
		}
		
		/// Picks a point on the surface of the terrain volume. 
		/**
		 * Note that the result may be slightly inaccurate due to the fixed step size.
		 */
		public static bool PickSurface(TerrainVolume volume, Vector3 origin, Vector3 direction, float distance, out PickSurfaceResult pickResult)
		{
			validateDistance(distance);
			
			// This 'out' value needs to be initialised even if we don't hit
			// anything (in which case it will be left at it's default value).
			pickResult = new PickSurfaceResult();
			
			// Can't hit it the volume if there's no data.
			if((volume.data == null) || (volume.data.volumeHandle == null))
			{
				return false;
			}
			
			// Cubiquity's picking code works in volume space whereas we expose an interface that works in world
			// space (for consistancy with other Unity functions). Therefore we apply the inverse of the volume's
			// volume-to-world transform to the ray, to bring it from world space into volume space.
			//
			// Note that we do this by transforming the start and end points of the ray (rather than the direction
			// of the ray) as Unity's Transform.InverseTransformDirection method does not handle scaling.
			Vector3 target = origin + direction * distance;				
			origin = volume.transform.InverseTransformPoint(origin);
			target = volume.transform.InverseTransformPoint(target);			
			direction = target - origin;
			
			// Now call through to the Cubiquity dll to do the actual picking.
			uint hit = CubiquityDLL.PickTerrainSurface((uint)volume.data.volumeHandle,
				origin.x, origin.y, origin.z,
				direction.x, direction.y, direction.z,
				out pickResult.volumeSpacePos.x, out pickResult.volumeSpacePos.y, out pickResult.volumeSpacePos.z);
			
			// The result is in volume space, but again it is more convienient for Unity users to have the result
			// in world space. Therefore we apply the volume's volume-to-world transform to the volume space position.
			pickResult.worldSpacePos = volume.transform.TransformPoint(pickResult.volumeSpacePos);
			
			// Return true if we hit a surface.
			return hit == 1;
		}
		
		// This funcion should be implemented to find the point where the ray
		// pierces the mesh, between the last empty voxel and the first solid voxel.
		/*public static bool PickSurface(ColoredCubesVolume volume, Vector3 origin, Vector3 direction, float distance, PickSurfaceResult pickResult)
		{
		}*/
		
		/// Picks the first solid voxel lying on the ray.
		/**
		 * 
		 */
		public static bool PickFirstSolidVoxel(ColoredCubesVolume volume, Ray ray, float distance, out PickVoxelResult pickResult)
		{
			return PickFirstSolidVoxel(volume, ray.origin, ray.direction, distance, out pickResult);
		}
		
		/// Picks the first solid voxel lying on the ray.
		/**
		 * 
		 */
		public static bool PickFirstSolidVoxel(ColoredCubesVolume volume, Vector3 origin, Vector3 direction, float distance, out PickVoxelResult pickResult)
		{			
			validateDistance(distance);
			
			// This 'out' value needs to be initialised even if we don't hit
			// anything (in which case it will be left at it's default value).
			pickResult = new PickVoxelResult();
			
			// Can't hit it the volume if there's no data.
			if((volume.data == null) || (volume.data.volumeHandle == null))
			{
				return false;
			}
			
			// Cubiquity's picking code works in volume space whereas we expose an interface that works in world
			// space (for consistancy with other Unity functions). Therefore we apply the inverse of the volume's
			// volume-to-world transform to the ray, to bring it from world space into volume space.
			//
			// Note that we do this by transforming the start and end points of the ray (rather than the direction
			// of the ray) as Unity's Transform.InverseTransformDirection method does not handle scaling.
			Vector3 target = origin + direction * distance;				
			origin = volume.transform.InverseTransformPoint(origin);
			target = volume.transform.InverseTransformPoint(target);			
			direction = target - origin;
			
			// Now call through to the Cubiquity dll to do the actual picking.
			uint hit = CubiquityDLL.PickFirstSolidVoxel((uint)volume.data.volumeHandle,
				origin.x, origin.y, origin.z,
				direction.x, direction.y, direction.z,
				out pickResult.volumeSpacePos.x, out pickResult.volumeSpacePos.y, out pickResult.volumeSpacePos.z);
			
			// The result is in volume space, but again it is more convienient for Unity users to have the result
			// in world space. Therefore we apply the volume's volume-to-world transform to the volume space position.
			pickResult.worldSpacePos = volume.transform.TransformPoint((Vector3)(pickResult.volumeSpacePos));
			
			// Return true if we hit a surface.
			return hit == 1;
		}
		
		/// Picks the last empty voxel lying on the ray.
		/**
		 * 
		 */
		public static bool PickLastEmptyVoxel(ColoredCubesVolume volume, Ray ray, float distance, out PickVoxelResult pickResult)
		{
			return PickLastEmptyVoxel(volume, ray.origin, ray.direction, distance, out pickResult);
		}
		
		/// Picks the last empty voxel lying on the ray.
		/**
		 * 
		 */
		public static bool PickLastEmptyVoxel(ColoredCubesVolume volume, Vector3 origin, Vector3 direction, float distance, out PickVoxelResult pickResult)
		{
			validateDistance(distance);
			
			// This 'out' value needs to be initialised even if we don't hit
			// anything (in which case it will be left at it's default value).
			pickResult = new PickVoxelResult();
			
			// Can't hit it the volume if there's no data.
			if((volume.data == null) || (volume.data.volumeHandle == null))
			{
				return false;
			}
			
			// Cubiquity's picking code works in volume space whereas we expose an interface that works in world
			// space (for consistancy with other Unity functions). Therefore we apply the inverse of the volume's
			// volume-to-world transform to the ray, to bring it from world space into volume space.
			//
			// Note that we do this by transforming the start and end points of the ray (rather than the direction
			// of the ray) as Unity's Transform.InverseTransformDirection method does not handle scaling.
			Vector3 target = origin + direction * distance;				
			origin = volume.transform.InverseTransformPoint(origin);
			target = volume.transform.InverseTransformPoint(target);			
			direction = target - origin;
			
			// Now call through to the Cubiquity dll to do the actual picking.
			pickResult = new PickVoxelResult();
			uint hit = CubiquityDLL.PickLastEmptyVoxel((uint)volume.data.volumeHandle,
				origin.x, origin.y, origin.z,
				direction.x, direction.y, direction.z,
				out pickResult.volumeSpacePos.x, out pickResult.volumeSpacePos.y, out pickResult.volumeSpacePos.z);
			
			// The result is in volume space, but again it is more convienient for Unity users to have the result
			// in world space. Therefore we apply the volume's volume-to-world transform to the volume space position.
			pickResult.worldSpacePos = volume.transform.TransformPoint((Vector3)(pickResult.volumeSpacePos));
			
			// Return true if we hit a surface.
			return hit == 1;
		}
	}
}
