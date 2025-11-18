import mongoose from 'mongoose';

const ProductSchema = new mongoose.Schema(
    {
        name: { type: String, required: true, trim: true },
        sku: { type: String, required: true, unique: true, index: true },
        description: { type: String },
        category: { type: String, index: true },
        size: { type: String },
        color: { type: String },
        price: { type: Number, required: true, min: 0 },
        images: [{ type: String }],
        model3dUrl: { type: String },
        stock: { type: Number, default: 1, min: 0 },
        active: { type: Boolean, default: true }
    },
    { timestamps: true }
);

ProductSchema.index({ name: 'text', description: 'text', category: 'text' });

export default mongoose.models.Product || mongoose.model('Product', ProductSchema);
