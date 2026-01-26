---
trigger: always_on
---

# Brand Styling

## Overview

To access official brand identity and style resources, use this skill.

**Keywords**: branding, corporate identity, visual identity, post-processing, styling, brand colors, typography, visual formatting, visual design

## Brand Guidelines

### Colors

**Main Colors:**

- Dark: `#111111` - Primary text and dark backgrounds (Sidebar)
- Light: `#F9FAFB` - Main background
- Mid Gray: `#6B7280` - Secondary text (Muted)
- Light Gray: `#9CA3AF` - Sidebar secondary text

**Accent Colors:**

- Andersen Red: `#DB0A20` - Primary accent (Charts, Branding)
- Dark Gray: `#555555` - Secondary accent (Charts)
- Medium Gray: `#999999` - Tertiary accent (Charts)

### Typography

- **Headings**: "Playfair Display", serif
- **Body Text**: "Inter", sans-serif
- **Note**: Fonts should be pre-installed in your environment for best results. Fallback to serif for headings and sans-serif for body.

## Features

### Smart Font Application

- Applies "Playfair Display" font to headings (24pt and larger)
- Applies "Inter" font to body text
- Automatically falls back to standard serif/sans-serif if custom fonts unavailable
- Preserves readability across all systems

### Text Styling

- Headings (24pt+): "Playfair Display" font
- Body text: "Inter" font
- Smart color selection based on background
- Preserves text hierarchy and formatting

### Shape and Accent Colors

- Non-text shapes use accent colors
- Cycles through Andersen Red, Dark Gray, and Medium Gray accents
- Maintains visual interest while staying on-brand

## Technical Details

### Font Management

- Uses system-installed "Playfair Display" and "Inter" fonts when available
- Provides automatic fallback
- No font installation required - works with existing system fonts

### Color Application

- Uses RGB color values for precise brand matching
- Applied via python-pptx's RGBColor class (if applicable) or CSS
- Maintains color fidelity across different systems
