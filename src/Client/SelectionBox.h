#ifndef CC_SELECTIONBOX_H
#define CC_SELECTIONBOX_H
#include "VertexStructs.h"
#include "Vectors.h"
/* Describes a selection box, and contains methods related to the selection box.
   Copyright 2014-2017 ClassicalSharp | Licensed under BSD-3
*/
struct IGameComponent;

void SelectionBox_VerQuad(VertexP3fC4b** vertices, PackedCol col,
	Real32 x1, Real32 y1, Real32 z1, Real32 x2, Real32 y2, Real32 z2);
void SelectionBox_HorQuad(VertexP3fC4b** vertices, PackedCol col,
	Real32 x1, Real32 z1, Real32 x2, Real32 z2, Real32 y);
void SelectionBox_Line(VertexP3fC4b** vertices, PackedCol col,
	Real32 x1, Real32 y1, Real32 z1, Real32 x2, Real32 y2, Real32 z2);

void Selections_MakeComponent(struct IGameComponent* comp);
void Selections_Render(Real64 delta);
void Selections_Add(UInt8 id, Vector3I p1, Vector3I p2, PackedCol col);
void Selections_Remove(UInt8 id);
#endif
