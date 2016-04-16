﻿using UnityEngine;
using System.Collections;

using Cubiquity;

/**
 * This class serves as an example of how to generate a TerrainVolume from code. The exact operation
 * of the noise function(s) is not particularly important here as you will want to implement your own
 * approach for your game, but you should focus on understanding how data is written into the volume.
 * Please note, most of the 'magic numbers' in this code are simply found by trial and error as there
 * is a lot of experimentation required to generate procedural terrains. Feel free to change them and
 * see what happens!
 */
[ExecuteInEditMode]
public class ProceduralTerrainVolume : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		// The size of the volume we will generate
		int width = 128;
		int height = 32;
		int depth = 128;
		
		// FIXME - Where should we delete this?
		/// [DoxygenSnippet-CreateEmptyTerrainVolumeData]
		//Create an empty TerrainVolumeData with dimensions width * height * depth
		TerrainVolumeData data = VolumeData.CreateEmptyVolumeData<TerrainVolumeData>(new Region(0, 0, 0, width-1, height-1, depth-1));
		/// [DoxygenSnippet-CreateEmptyTerrainVolumeData]
		
		TerrainVolume volume = GetComponent<TerrainVolume>();
		TerrainVolumeRenderer volumeRenderer = GetComponent<TerrainVolumeRenderer>();
		
		volume.data = data;
		
		// This example looks better if we adjust the scaling factors on the textures.
		volumeRenderer.material.SetTextureScale("_Tex0", new Vector2(0.062f, 0.062f));
		volumeRenderer.material.SetTextureScale("_Tex1", new Vector2(0.125f, 0.125f));	
		volumeRenderer.material.SetTextureScale("_Tex2", new Vector2(0.125f, 0.125f));
		
		// At this point our volume is set up and ready to use. The remaining code is responsible
		// for iterating over all the voxels and filling them according to our noise functions.
		
		// This scale factor comtrols the size of the rocks which are generated.
		float rockScale = 32.0f;		
		float invRockScale = 1.0f / rockScale;
		
		// Let's keep the allocation outside of the loop.
		MaterialSet materialSet = new MaterialSet();
		
		// Iterate over every voxel of our volume
		for(int z = 0; z < depth; z++)
		{
			for(int y = height-1; y > 0; y--)
			{
				for(int x = 0; x < width; x++)
				{
					// Make sure we don't have anything left in here from the previous voxel
					materialSet.weights[0] = 0;
					materialSet.weights[1] = 0;
					materialSet.weights[2] = 0;
					
					// Simplex noise is quite high frequency. We scale the sample position to reduce this.
					float sampleX = (float)x * invRockScale;
					float sampleY = (float)y * invRockScale;
					float sampleZ = (float)z * invRockScale;
					
					// Get the noise value for the current position.
					// Returned value should be in the range -1 to +1.
					float simplexNoiseValue = SimplexNoise.Noise.Generate(sampleX, sampleY, sampleZ);
					
					// We want to fade off the noise towards the top of the volume (so that the rocks don't go
					// up to the sky) adn add extra material near the bottom of the volume (to create a floor).
					// This altitude value is initially in the range from 0 to +1.
					float altitude = (float)(y + 1) / (float)height;
					
					// Map the altitude to the range -1.0 to +1.0...
					altitude = (altitude * 2.0f) - 1.0f;
					
					// Subtract the altitude from the noise. This adds
					// material near the ground and subtracts it higher up.					
					simplexNoiseValue -= altitude;
					
					// After combining our noise value and our altitude we now have values between -2.0 and 2.0.
					// Cubiquity renders anything below the threshold as empty and anythng above as solid, but
					// in general it is easiest if empty space is completly empty and solid space is completly
					// solid. The exception to this is the region near our surface, where a gentle transition helps
					// obtain smooth shading. By scaling by a large number and then clamping we achieve this effect
					// of making most voxels fully solid or fully empty except near the surface..
					simplexNoiseValue *= 5.0f;
					simplexNoiseValue = Mathf.Clamp(simplexNoiseValue, -0.5f, 0.5f);
					
					// Go back to the range 0.0 to 1.0;
					simplexNoiseValue += 0.5f;
					
					// And then to 0 to 255, ready to convert into a byte.
					simplexNoiseValue *= 255;
					
					// Write the final value value into the third material channel (the one with the rock texture).
					// The value being written is usually 0 (empty) or 255 (solid) except around the transition.
					materialSet.weights[2] = (byte)simplexNoiseValue;
					
					
					// Lastly we write soil or grass voxels into the volume to create a level floor between the rocks.
					// This means we want to set the sum of the materials to 255 if the voxel is below the floor height.
					// We don't want to interfere with the rocks on the transition between the material so we work out
					// how much extra we have to add to get to 255 and then add that to either soil or grass.
					byte excess = (byte)(255 - materialSet.weights[2]);					
					if(y < 11)
					{
						// Add to soil material channel.
						materialSet.weights[1] = excess;
					}
					else if(y < 12)
					{
						// Add to grass material channel.
						materialSet.weights[0] = excess;
					}
					
					// We can now write our computed voxel value into the volume.
					data.SetVoxel(x, y, z, materialSet);
				}
			}
		}
	}
}
